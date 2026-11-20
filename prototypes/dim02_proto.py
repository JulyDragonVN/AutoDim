# DIM02 prototype: tao dim cho tam thep (OUT + bolt holes) tu origin.dxf, so voi dim.dxf
# Dung: python prototypes/dim02_proto.py origin.dxf dim.dxf   (can pip install ezdxf)
import ezdxf, math, sys, re
TOL=1.0; AX=3.0   # mm ; deg (canh coi la ngang/dung)
CHAMFER_MAX=50.0
CFG=dict(tier1=50.0, step=45.0, arcR=150.0)

def P(v): return (float(v[0]),float(v[1]))
def dist(a,b): return math.hypot(a[0]-b[0],a[1]-b[1])
def same(a,b,t=TOL): return dist(a,b)<=t

class Edge:
    def __init__(s,kind,p,q,c=None,r=None,a0=None,a1=None):
        s.kind,s.p,s.q,s.c,s.r,s.a0,s.a1=kind,p,q,c,r,a0,a1
    @property
    def dx(s): return s.q[0]-s.p[0]
    @property
    def dy(s): return s.q[1]-s.p[1]
    @property
    def length(s): return math.hypot(s.dx,s.dy)
    def ang(s):  # goc so voi truc x, 0..180
        return math.degrees(math.atan2(s.dy,s.dx))%180
    def is_h(s): a=s.ang(); return s.kind=='L' and min(a,180-a)<=AX
    def is_v(s): return s.kind=='L' and abs(s.ang()-90)<=AX
    def is_slant(s): return s.kind=='L' and not s.is_h() and not s.is_v()
    def touches(s,pt): return same(s.p,pt) or same(s.q,pt)

def load(path):
    doc=ezdxf.readfile(path); ms=doc.modelspace()
    borders=[]; edges=[]; holes=[]; texts=[]
    for e in ms:
        t=e.dxftype()
        if t=='LWPOLYLINE' and e.dxf.layer=='BORDER':
            pts=[P(p) for p in e.get_points()]
            xs=[p[0] for p in pts]; ys=[p[1] for p in pts]
            borders.append((min(xs),min(ys),max(xs),max(ys)))
        elif t=='LINE' and e.dxf.layer=='OUT':
            edges.append(Edge('L',P(e.dxf.start),P(e.dxf.end)))
        elif t=='ARC' and e.dxf.layer=='OUT':
            c=P(e.dxf.center); r=e.dxf.radius; a0=e.dxf.start_angle; a1=e.dxf.end_angle
            p=(c[0]+r*math.cos(math.radians(a0)),c[1]+r*math.sin(math.radians(a0)))
            q=(c[0]+r*math.cos(math.radians(a1)),c[1]+r*math.sin(math.radians(a1)))
            edges.append(Edge('A',p,q,c,r,a0,a1))
        elif t=='CIRCLE':
            holes.append((P(e.dxf.center),e.dxf.radius))
        elif t=='TEXT':
            texts.append((P(e.dxf.insert),e.dxf.text))
    return borders,edges,holes,texts

def part_layer(b,texts):
    # layer = 'check ' + ma chi tiet (P=GV41 -> GV), bo hau to so seri
    for pt,tx in texts:
        if inb(b,pt) and tx.strip().upper().startswith('P='):
            code=re.sub(r'\d+$','',tx.strip()[2:].strip())
            return 'check '+code
    return None

def inb(b,pt): return b[0]<=pt[0]<=b[2] and b[1]<=pt[1]<=b[3]

# ---- spec: dict(type, ...)
def lin(p1,p2,ang,line): return dict(t='LIN',p1=p1,p2=p2,ang=ang,line=line)

def gen_plate(edges,holes,cfg=CFG):
    specs=[]
    verts=[]; 
    for e in edges: verts+= [e.p,e.q]
    xmin=min(v[0] for v in verts); xmax=max(v[0] for v in verts)
    ymin=min(v[1] for v in verts); ymax=max(v[1] for v in verts)
    cx=sum(v[0] for v in verts)/len(verts); cy=sum(v[1] for v in verts)/len(verts)
    tiers={'L':0,'R':0,'T':0,'B':0}
    def off(side):
        tiers[side]+=1
        return cfg['tier1']+(tiers[side]-1)*cfg['step']
    def lineat(side):
        o=off(side)
        return {'L':xmin-o,'R':xmax+o,'T':ymax+o,'B':ymin-o}[side]
    lines=[e for e in edges if e.kind=='L']
    hor=[e for e in lines if e.is_h()]
    longest=max(hor,key=lambda e:e.length) if hor else None
    wside='T' if longest and abs(longest.p[1]-ymax)<=TOL else 'B'
    dside=wside
    _ar=[e for e in edges if e.kind=='A']
    _la=[a for a in _ar if a.c[0]<cx]; _ra=[a for a in _ar if a.c[0]>cx]
    if _la and _ra: dside='B' if (min(_la,key=lambda a:a.c[0]).c[1]+max(_ra,key=lambda a:a.c[0]).c[1])/2<cy else 'T'
    # 1) chamfer nho (canh xien ngan): chan ngang + chan dung
    for e in lines:
        if e.is_slant() and e.length<=CHAMFER_MAX:
            hs='B' if abs(e.p[1]-ymin)<=TOL+abs(e.dy) and (e.p[1]+e.q[1])/2<cy else 'T'
            vs='L' if (e.p[0]+e.q[0])/2<cx else 'R'
            specs.append(lin(e.p,e.q,0,lineat(hs)))
            specs.append(lin(e.p,e.q,90,lineat(vs)))
    # 2) canh xien lon: goc voi canh ngang dai + canh dung ke no
    for e in lines:
        if e.is_slant() and e.length>CHAMFER_MAX and longest is not None:
            specs.append(dict(t='ANG',l1=(longest.p,longest.q),l2=(e.p,e.q)))
            for v in lines:
                if v.is_v() and (v.touches(e.p) or v.touches(e.q)):
                    side='L' if (v.p[0]+v.q[0])/2<cx else 'R'
                    specs.append(lin(v.p,v.q,90,lineat(side)))
    # 3) cung: ban kinh + goc canh dung ke cung voi canh ngang + lech ngang
    for a in edges:
        if a.kind!='A': continue
        amid=math.radians((a.a0+ (a.a1 if a.a1>a.a0 else a.a1+360))/2)
        specs.append(dict(t='RAD',c=a.c,r=a.r,pt=(a.c[0]+a.r*math.cos(amid),a.c[1]+a.r*math.sin(amid))))
        for v in lines:
            if v.is_v() and (v.touches(a.p) or v.touches(a.q)):
                other=a.q if v.touches(a.p) else a.p
                for h in hor:
                    if h.touches(other):
                        specs.append(dict(t='ANG',l1=(h.p,h.q),l2=(v.p,v.q)))
                        far=v.q if dist(v.p,a.c)<dist(v.q,a.c) else v.p
                        if abs(far[0]-a.c[0])>TOL:
                            specs.append(lin(far,a.c,0,lineat('B' if wside=='T' else 'T')))
    # 4) lo bulong: chuoi ngang (phia doi dien rong tong) toi dau phai + chuoi dung trai va phai
    if holes:
        cs=sorted([h[0] for h in holes])
        lefts=[v for v in verts if v[0]<=xmin+TOL]; rights=[v for v in verts if v[0]>=xmax-TOL]
        bl=min(lefts,key=lambda v:v[1]); tl=max(lefts,key=lambda v:v[1])
        br=min(rights,key=lambda v:v[1]); tr=max(rights,key=lambda v:v[1])
        hside='B' if dside=='T' else 'T'
        ly=lineat(hside)
        pts=[bl if hside=='B' else tl]+cs+[br if hside=='B' else tr]
        for i in range(len(pts)-1): specs.append(lin(pts[i],pts[i+1],0,ly))
        def chain(order,c0,c1,side):
            lv=lineat(side); levels=[]
            for c in order:
                if not any(abs(c[1]-l[1])<=TOL for l in levels): levels.append(c)
            levels.sort(key=lambda c:c[1])
            vp=[c0]+levels+[c1]
            for i in range(len(vp)-1): specs.append(lin(vp[i],vp[i+1],90,lv))
        if min(c[0] for c in cs)-xmin <= xmax-max(c[0] for c in cs): chain(cs,bl,tl,'L')   # lo gan canh trai: chi chuoi trai
        else: chain(list(reversed(cs)),br,tr,'R')                                          # lo gan canh phai: chi chuoi phai
    # 5) kich thuoc tong (sau cung de len tang ngoai)
    def pick(cands,key): return key(cands)
    lefts=[v for v in verts if v[0]<=xmin+TOL]; rights=[v for v in verts if v[0]>=xmax-TOL]
    if wside=='T': pl=max(lefts,key=lambda v:v[1]); pr=max(rights,key=lambda v:v[1])
    else: pl=min(lefts,key=lambda v:v[1]); pr=min(rights,key=lambda v:v[1])
    arcs=[e for e in edges if e.kind=='A']
    la=[a for a in arcs if a.c[0]<cx]; ra=[a for a in arcs if a.c[0]>cx]
    if la and ra:   # 2 goc trai/phai co cung scallop: do giua 2 tam cung
        pl=min(la,key=lambda a:a.c[0]).c; pr=max(ra,key=lambda a:a.c[0]).c
    specs.append(lin(pr,pl,0,lineat(dside)))
    tops=[v for v in verts if v[1]>=ymax-TOL]; bots=[v for v in verts if v[1]<=ymin+TOL]
    pt_=min(tops,key=lambda v:v[0]); pb=min(bots,key=lambda v:v[0])
    specs.append(lin(pt_,pb,90,lineat('L')))
    return specs

# ---- do goc theo 2 duong + diem cung
def ang_measure(l1,l2,arcpt):
    (a,b),(c,d)=l1,l2
    u=(b[0]-a[0],b[1]-a[1]); v=(d[0]-c[0],d[1]-c[1])
    den=u[0]*v[1]-u[1]*v[0]
    if abs(den)<1e-9: return None
    t=((c[0]-a[0])*v[1]-(c[1]-a[1])*v[0])/den
    I=(a[0]+t*u[0],a[1]+t*u[1])
    w=(arcpt[0]-I[0],arcpt[1]-I[1]); aw=math.atan2(w[1],w[0])
    best=None
    for su in (1,-1):
        for sv in (1,-1):
            a1=math.atan2(su*u[1],su*u[0]); a2=math.atan2(sv*v[1],sv*v[0])
            diff=(a2-a1+math.pi)%(2*math.pi)-math.pi   # -pi..pi
            lo,hi=(a1,a1+diff) if diff>=0 else (a1+diff,a1)
            rel=(aw-lo)%(2*math.pi)
            if rel<=abs(diff): best=math.degrees(abs(diff))
    return best

def ref_dims(path):
    doc=ezdxf.readfile(path); out=[]
    for e in doc.modelspace().query('DIMENSION'):
        x=e.dxf; t=x.dimtype&7
        if t==0:
            a=(x.get('angle',0) or 0)%180
            ln=P(x.defpoint)[1] if a<1 else P(x.defpoint)[0]
            out.append(dict(t='LIN',p1=P(x.defpoint2),p2=P(x.defpoint3),ang=a,line=ln,h=x.handle,layer=x.layer,style=x.dimstyle))
        elif t==2:
            l1=(P(x.defpoint2),P(x.defpoint3)); l2=(P(x.defpoint4),P(x.defpoint))
            out.append(dict(t='ANG',l1=l1,l2=l2,m=ang_measure(l1,l2,P(x.defpoint5)),h=x.handle,layer=x.layer,style=x.dimstyle))
        elif t==4:
            out.append(dict(t='RAD',c=P(x.defpoint),pt=P(x.defpoint4),h=x.handle,layer=x.layer,style=x.dimstyle))
    return out

def seg_same(s1,s2): return (same(s1[0],s2[0]) and same(s1[1],s2[1])) or (same(s1[0],s2[1]) and same(s1[1],s2[0]))
def gen_match(g,r):
    if g['t']!=r['t']: return False
    if g['t']=='LIN':
        if abs(g['ang']%180-r['ang'])>1: return False
        return (same(g['p1'],r['p1']) and same(g['p2'],r['p2'])) or (same(g['p1'],r['p2']) and same(g['p2'],r['p1']))
    if g['t']=='ANG':
        return (seg_same(g['l1'],r['l1']) and seg_same(g['l2'],r['l2'])) or (seg_same(g['l1'],r['l2']) and seg_same(g['l2'],r['l1']))
    if g['t']=='RAD': return same(g['c'],r['c'])
    return False

def main(origin='origin.dxf',dimf='dim.dxf'):
    borders,edges,holes,texts=load(origin)
    refs=ref_dims(dimf)
    gens=[]
    for b in borders:
        es=[e for e in edges if inb(b,e.p) and inb(b,e.q)]
        hs=[h for h in holes if inb(b,h[0])]
        sp=gen_plate(es,hs); 
        lay=part_layer(b,texts)
        if lay is None: print('  LOI: khong thay text P= trong khung'); continue
        for s in sp: s['plate']=b; s['layer']=lay
        print('  layer',lay)
        gens+=sp
        print('plate',[round(v) for v in b],'edges',len(es),'holes',len(hs),'-> gen',len(sp))
    used=set(); ok=0; rows=[]
    for r in refs:
        m=None
        for i,g in enumerate(gens):
            if i not in used and gen_match(g,r): m=i; break
        if m is None: rows.append((r['h'],r['t'],'KHONG CO TRONG GEN')); continue
        used.add(m); g=gens[m]; ok+=1
        extra=''
        if r['t']=='LIN': extra='dimline lech %.1f mm'%abs(g['line']-r['line'])
        if r['t']=='ANG':
            arc=None
            extra='goc ref=%.2f'%r['m']
        rows.append((r['h'],r['t'],'khop '+extra))
    print('KHOP QUY TAC (diem goc/loai/huong):',ok,'/',len(refs))
    for r in rows: print('  ',*r)
    print('GEN THUA (khong co trong ref):',len(gens)-len(used))
    for i,g in enumerate(gens):
        if i not in used: print('  ',g['t'],{k:v for k,v in g.items() if k not in('plate',)})
    return gens,refs
if __name__=='__main__':
    main(*(sys.argv[1:3]))
