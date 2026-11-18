import math
from proto import *
def key(dim_kind,p1,p2,dl,ang):
    """return (measure-coords tuple, dimline coord)"""
    a=math.radians(ang)
    if dim_kind=='A':
        dx,dy=p2[0]-p1[0],p2[1]-p1[1]; L=math.hypot(dx,dy); ux,uy=dx/L,dy/L
        n=((dl[0]-p1[0])*(-uy)+(dl[1]-p1[1])*ux)
        return ('A',tuple(sorted([(round(p1[0],0),round(p1[1],0)),(round(p2[0],0),round(p2[1],0))])), n)
    ux,uy=math.cos(a),math.sin(a)
    t1=p1[0]*ux+p1[1]*uy; t2=p2[0]*ux+p2[1]*uy
    n=(dl[0]*(-uy)+dl[1]*ux)
    return ('R',round(a,2)%3.1416 if False else None, sorted([t1,t2]), n)
ORIGIN_FILE='origin.dxf'; DIM_FILE='dim.dxf'
def expected():
    d,ms=load(ORIGIN_FILE); d2,ms2=load(DIM_FILE)
    h0={e.dxf.handle for e in ms}
    ex=[]
    for e in ms2:
        if e.dxf.handle in h0: continue
        x=e.dxf
        kind='A' if x.dimtype&7==1 else 'R'
        ex.append(Dim(kind,x.layer,x.dimstyle,R(P(x.defpoint2)),R(P(x.defpoint3)),R(P(x.defpoint)),x.get('angle',0.0) if kind=='R' else 0,tag=x.handle))
    return ex
def match(gen,ex,tolm=1.0,tolp=1.0,verbose=True):
    used=set(); res=[]
    for e in ex:
        best=None
        for i,g in enumerate(gen):
            if i in used or g.layer!=e.layer or g.kind!=e.kind: continue
            ke=key(e.kind,e.p1,e.p2,e.dl,e.angle); kg=key(g.kind,g.p1,g.p2,g.dl,g.angle)
            if e.kind=='A':
                # compare endpoints: distance
                sc=max(math.dist(a,b) for a,b in zip(sorted([e.p1,e.p2]),sorted([g.p1,g.p2])))
                pos=abs(abs(ke[2])-abs(kg[2]))
            else:
                if abs(((e.angle-g.angle+90)%180)-90)>0.1: continue
                sc=max(abs(a-b) for a,b in zip(ke[2],kg[2])); pos=abs(ke[3]-kg[3])
            if best is None or sc+pos*0.001<best[0]+best[2]*0.001: best=(sc,i,pos)
        if best and best[0]<tolm:
            used.add(best[1]); res.append((e,gen[best[1]],best[0],best[2]))
        else: res.append((e,None,None,None))
    return res,[g for i,g in enumerate(gen) if i not in used]
if __name__=='__main__':
    d,ms=load('origin.dxf'); g=read(ms); gen,_,_,_=dim01(g); ex=expected()
    res,extra=match(gen,ex)
    ok=[r for r in res if r[1]]; okp=[r for r in ok if r[3]<=1.0]
    print('expected',len(ex),'generated',len(gen),'matched points/measure',len(ok),'also position<=1mm',len(okp))
    print('--- not matched'); 
    for r in res:
        if not r[1]: print(r[0].layer,r[0].style,r[0].kind,r[0])
    print('--- extra generated'); 
    for x in extra: print(x)
    print('--- position diffs >1mm (ex.tag, gen.tag, diff)')
    for r in ok:
        if r[3]>1.0: print(r[0].layer,r[1].tag,r[0].tag,round(r[3],1))
