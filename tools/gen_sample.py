"""Sinh file du lieu mau C# cho test xUnit tu cap DXF (goc + da do).
Dung: python tools/gen_sample.py origin.dxf dim.dxf Sample01 > tests/AutoDimPanel.Tests/Sample01.cs
Toa do ghi la toa do that (tuyet doi) trong DXF.
Can: pip install ezdxf
"""
import os, sys
sys.path.insert(0, os.path.join(os.path.dirname(__file__), '..', 'prototypes'))
import load as L
L.U = ''
from load import load, P
import proto, compare

def f(v): return repr(float(v))
def pt(p): return 'new Pt(%s, %s)' % (f(p[0]), f(p[1]))

def main(origin, dim, cls):
    compare.ORIGIN_FILE = origin
    compare.DIM_FILE = dim
    d, ms = load(origin)
    g = proto.read(ms)
    gen, _, _, _ = proto.dim01(g)
    ex = compare.expected()           # toa do tuong doi
    res, _ = compare.match(gen, ex)
    hand = sorted(r[0].tag for r in res if r[1] is None or r[3] > 1.0)
    ox, oy = proto.ORG
    def A(p): return (p[0] + ox, p[1] + oy)
    out = []
    w = out.append
    w('// File sinh tu dong boi tools/gen_sample.py - khong sua tay.')
    w('using System.Collections.Generic;')
    w('using AutoDimPanel.Core;')
    w('')
    w('namespace AutoDimPanel.Tests')
    w('{')
    w('    public sealed class ExpectedDim')
    w('    {')
    w('        public string Handle; public DimKind Kind; public string Layer; public string Style;')
    w('        public Pt P1; public Pt P2; public Pt LinePoint; public double RotationDeg; public bool HandPlaced;')
    w('    }')
    w('')
    w('    public static class %s' % cls)
    w('    {')
    w('        public static Dim01Input Input()')
    w('        {')
    w('            Dim01Input r = new Dim01Input();')
    dd, ms0 = load(origin)
    for e in ms0:
        if e.dxftype() == 'LWPOLYLINE' and e.dxf.layer == 'CHECK':
            pts = ', '.join(pt((float(x), float(y))) for x, y, *_ in e.get_points())
            w('            r.CheckPolylines.Add(new List<Pt> { %s });' % pts)
    for e in ms0:
        if e.dxftype() == 'LINE' and e.dxf.layer in ('CHECK', 'MRK1'):
            w('            r.Lines.Add(new Seg(%s, %s, "%s"));' % (pt(P(e.dxf.start)), pt(P(e.dxf.end)), e.dxf.layer))
    for e in ms0:
        if e.dxftype() == 'CIRCLE' and e.dxf.layer == 'BOLT':
            w('            r.Bolts.Add(%s);' % pt(P(e.dxf.center)))
    w('            return r;')
    w('        }')
    w('')
    w('        public static List<ExpectedDim> Expected()')
    w('        {')
    w('            List<ExpectedDim> r = new List<ExpectedDim>();')
    for e in ex:
        w('            r.Add(new ExpectedDim { Handle = "%s", Kind = DimKind.%s, Layer = "%s", Style = "%s", P1 = %s, P2 = %s, LinePoint = %s, RotationDeg = %s, HandPlaced = %s });' % (
            e.tag, 'Aligned' if e.kind == 'A' else 'Rotated', e.layer, e.style,
            pt(A(e.p1)), pt(A(e.p2)), pt(A(e.dl)), f(e.angle), 'true' if e.tag in hand else 'false'))
    w('            return r;')
    w('        }')
    w('    }')
    w('}')
    print('\n'.join(out))

if __name__ == '__main__':
    main(sys.argv[1], sys.argv[2], sys.argv[3])
