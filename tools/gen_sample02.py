"""Sinh du lieu mau C# cho test DIM02 tu cap DXF (goc + da do).
Dung: python tools/gen_sample02.py origin.dxf dim.dxf Sample02 > tests/AutoDimPanel.Tests/Sample02.cs
Toa do ghi la toa do that (tuyet doi). Moi khung BORDER la mot tam (theo thu tu trong file).
Can: pip install ezdxf
"""
import os, sys
sys.path.insert(0, os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', 'prototypes'))
import dim02_proto as D

def f(v): return repr(float(v))
def pt(p): return 'new Pt(%s, %s)' % (f(p[0]), f(p[1]))

def main(origin, dim, cls):
    borders, edges, holes, texts = D.load(origin)
    refs = D.ref_dims(dim)
    out = []
    w = out.append
    w('// File sinh tu dong boi tools/gen_sample02.py - khong sua tay.')
    w('using System.Collections.Generic;')
    w('using AutoDimPanel.Core;')
    w('')
    w('namespace AutoDimPanel.Tests')
    w('{')
    w('    public sealed class Expected02')
    w('    {')
    w('        public string Handle; public DimKind Kind; public Pt P1; public Pt P2; public Pt LinePoint; public double RotationDeg;')
    w('        public Pt A1; public Pt A2; public Pt B1; public Pt B2; public Pt Center;')
    w('    }')
    w('')
    w('    public static class %s' % cls)
    w('    {')
    w('        public const int PlateCount = %d;' % len(borders))
    w('')
    w('        public static Dim02Input Input(int plate)')
    w('        {')
    w('            Dim02Input r = new Dim02Input();')
    w('            switch (plate)')
    w('            {')
    for i, b in enumerate(borders):
        w('                case %d:' % i)
        code = ''
        for p, tx in texts:
            if D.inb(b, p) and tx.strip().upper().startswith('P='):
                code = tx.strip()[2:].strip()
        w('                    r.PartCode = "%s";' % code)
        for e in edges:
            if not (D.inb(b, e.p) and D.inb(b, e.q)): continue
            if e.kind == 'L':
                w('                    r.Lines.Add(new Seg(%s, %s, "OUT"));' % (pt(e.p), pt(e.q)))
            else:
                w('                    r.Arcs.Add(new ArcSeg(%s, %s, %s, %s));' % (pt(e.c), f(e.r), f(e.a0), f(e.a1)))
        for c, rad in holes:
            if D.inb(b, c):
                w('                    r.Holes.Add(%s);' % pt(c))
        w('                    break;')
    w('            }')
    w('            return r;')
    w('        }')
    w('')
    w('        public static List<Expected02> Expected(int plate)')
    w('        {')
    w('            List<Expected02> r = new List<Expected02>();')
    w('            switch (plate)')
    w('            {')
    assigned = 0
    for i, b in enumerate(borders):
        w('                case %d:' % i)
        for d in refs:
            anchor = d['p1'] if d['t'] == 'LIN' else (d['l1'][0] if d['t'] == 'ANG' else d['c'])
            if not D.inb(b, anchor): continue
            assigned += 1
            if d['t'] == 'LIN':
                w('                    r.Add(new Expected02 { Handle = "%s", Kind = DimKind.Rotated, P1 = %s, P2 = %s, RotationDeg = %s });' % (d['h'], pt(d['p1']), pt(d['p2']), f(d['ang'])))
            elif d['t'] == 'ANG':
                w('                    r.Add(new Expected02 { Handle = "%s", Kind = DimKind.Angular, A1 = %s, A2 = %s, B1 = %s, B2 = %s });' % (d['h'], pt(d['l1'][0]), pt(d['l1'][1]), pt(d['l2'][0]), pt(d['l2'][1])))
            else:
                w('                    r.Add(new Expected02 { Handle = "%s", Kind = DimKind.Radial, Center = %s });' % (d['h'], pt(d['c'])))
        w('                    break;')
    w('            }')
    w('            return r;')
    w('        }')
    w('    }')
    w('}')
    sys.stderr.write('assigned %d / %d ref dims\n' % (assigned, len(refs)))
    sys.stdout.reconfigure(encoding='utf-8')
    print('\n'.join(out))

if __name__ == '__main__':
    main(sys.argv[1], sys.argv[2], sys.argv[3])
