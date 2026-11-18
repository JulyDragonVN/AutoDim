import math
from load import *
ORG=[0.0,0.0]
def R(p): return (p[0]-ORG[0],p[1]-ORG[1])

class Dim:
    def __init__(s,kind,layer,style,p1,p2,dl,angle=0.0,tag=''):
        s.kind=kind; s.layer=layer; s.style=style; s.p1=p1; s.p2=p2; s.dl=dl; s.angle=angle; s.tag=tag
    def __repr__(s): return f'{s.tag}:{s.kind}:{s.layer}:{s.style}:{tuple(round(v,1) for v in s.p1)}->{tuple(round(v,1) for v in s.p2)}@{tuple(round(v,1) for v in s.dl)}/{round(s.angle,2)}'

def interp(poly,x):
    pts=sorted(poly)
    for a,b in zip(pts,pts[1:]):
        if a[0]-1e-6<=x<=b[0]+1e-6:
            t=(x-a[0])/(b[0]-a[0]); return (x,a[1]+t*(b[1]-a[1]))

def read(ms):
    g={}
    b=[e for e in ms if e.dxftype()=='LWPOLYLINE' and e.dxf.layer=='BORDER'][0]
    pts=[(float(x),float(y)) for x,y,*_ in b.get_points()]
    ORG[0]=min(p[0] for p in pts); ORG[1]=min(p[1] for p in pts)
    chk=[[R((float(x),float(y))) for x,y,*_ in e.get_points()] for e in ms if e.dxftype()=='LWPOLYLINE' and e.dxf.layer=='CHECK']
    chk.sort(key=lambda p:sum(q[1] for q in p)/len(p))
    g['bottom'],g['top']=sorted(chk[0]),sorted(chk[1])
    g['lines']=[(R(P(e.dxf.start)),R(P(e.dxf.end)),e.dxf.layer) for e in ms if e.dxftype()=='LINE']
    g['bolts']=[R(P(e.dxf.center)) for e in ms if e.dxftype()=='CIRCLE' and e.dxf.layer=='BOLT']
    return g

CFG=dict(vs_row=269.0, kote_row=369.6, kote_off=86.3, out_L=342.2,out_R=227.8, hs_L=189.0, hs_R=125.1,
         cb_off=80.1, thick_cb=70.0, thick_vs=97.5, plate_w_in=45.0, vchain_off=66.5, corner_off=100.0)

def dim01(g,C=CFG):
    out=[]; bot,top=g['bottom'],g['top']
    L=g['lines']
    chk=[(a,b) for a,b,l in L if l=='CHECK']; mrk=[(a,b) for a,b,l in L if l=='MRK1']
    xl,xr=bot[0][0],bot[-1][0]; ymin=min(p[1] for p in bot)
    # OUT
    out.append(Dim('A','check out','DIM50',bot[0],top[0],(xl-C['out_L'],top[0][1]),tag='out'))
    out.append(Dim('A','check out','DIM50',bot[-1],top[-1],(xr+C['out_R'],top[-1][1]),tag='out'))
    # v-stiff chain
    vert=[(a,b) for a,b in chk if abs(a[0]-b[0])<0.5 and abs(a[1]-b[1])>1000]
    xs=sorted({round(a[0],1) for a,b in vert}); xs=[x for x in xs if abs(x-xr)>1]
    pts=[]
    for x in xs:
        ls=[(a,b) for a,b in vert if abs(a[0]-x)<1]
        pts.append((ls[0][0][0],min(min(a[1],b[1]) for a,b in ls)))
    for a,b in zip(pts,pts[1:]):
        out.append(Dim('R','check v-stiff','DIM50',a,b,(b[0],ymin-C['vs_row']),0,tag='vs'))
    # kote
    cols={}
    for b in g['bolts']: cols.setdefault(round(b[0],1),[]).append(b)
    cx=sorted(cols); up=[max(cols[x],key=lambda p:p[1]) for x in cx]
    chain=[bot[0]]+up+[bot[-1]]
    for a,b in zip(chain,chain[1:]):
        out.append(Dim('R','check kote','DIM50',a,b,(b[0],ymin-C['kote_row']),0,tag='kote-h'))
    for x in cx:
        u,l=sorted(cols[x],key=lambda p:-p[1]); f=interp(bot,u[0])
        for a,b in ((u,l),(l,f)):
            out.append(Dim('R','check kote','DIM15',a,b,(a[0]+C['kote_off'],b[1]),90,tag='kote-v'))
    # plates (MRK1 near-horizontal long)
    pl=[(a,b) for a,b in mrk if math.hypot(b[0]-a[0],b[1]-a[1])>300 and abs(b[1]-a[1])<0.1*abs(b[0]-a[0])]
    plates=[]
    for a,b in pl:
        a,b=sorted([a,b]); xm=(a[0]+b[0])/2
        for p in plates:
            if abs(p['x0']-a[0])<1.5 and abs(p['x1']-b[0])<1.5: p['ls'].append((a,b)); break
        else: plates.append(dict(x0=a[0],x1=b[0],ls=[(a,b)]))
    plates.sort(key=lambda p:p['x0'])
    for p in plates:
        ls=sorted(p['ls'],key=lambda l:l[0][1])
        lo,hi=ls[0],ls[-1]
        p['L']=(lo[0],hi[0]); p['Rr']=(lo[1],hi[1])     # (lower face end, upper face end)
        p['t']=hi[0][1]-lo[0][1]
        p['xm0']=(lo[0][0]+hi[0][0])/2; p['xm1']=(lo[1][0]+hi[1][0])/2
        p['ym0']=(lo[0][1]+hi[0][1])/2; p['ym1']=(lo[1][1]+hi[1][1])/2
    # h-stiff row (CHECK non-vertical lines near plates)
    hl=[(a,b) for a,b in chk if abs(a[0]-b[0])>1 and abs(b[1]-a[1])<0.1*abs(b[0]-a[0]) and abs(b[0]-a[0])>400]
    hl=[tuple(sorted(l)) for l in hl]
    pm=plates[0]['ym0']
    row=sorted([l for l in hl if abs(l[0][1]-pm)<60 or any(abs(l[0][1]-p['ym0'])<60 for p in plates)])
    row=sorted(set(row))
    # left/right edges chain on h-stiff
    top_c=top[0]; 
    out.append(Dim('R','check h-stiff','DIM50',top[0],row[0][0],(xl-C['hs_L'],row[0][0][1]),90,tag='hs-edge'))
    out.append(Dim('R','check h-stiff','DIM50',row[0][0],bot[0],(xl-C['hs_L'],bot[0][1]),90,tag='hs-edge'))
    out.append(Dim('R','check h-stiff','DIM50',top[-1],row[-1][1],(xr+C['hs_R'],row[-1][1][1]),90,tag='hs-edge'))
    out.append(Dim('R','check h-stiff','DIM50',row[-1][1],bot[-1],(xr+C['hs_R'],bot[-1][1]),90,tag='hs-edge'))
    # members (vertical MRK1)
    vm=[(a,b) for a,b in mrk if abs(a[0]-b[0])<0.5 and abs(a[1]-b[1])>300]
    mem=[]
    for a,b in vm:
        xm=a[0]
        for m in mem:
            if abs(m['xc']-xm)<12: m['ls'].append((a,b)); break
        else: mem.append(dict(xc=xm,ls=[(a,b)]))
    for m in mem:
        xs_=[l[0][0] for l in m['ls']]; m['xc']=(min(xs_)+max(xs_))/2
    mem.sort(key=lambda m:m['xc'])
    # gusset horizontal plates 100 long
    hz=[(a,b) for a,b in mrk if abs(a[1]-b[1])<0.1 and abs(abs(b[0]-a[0])-100)<1]
    for m in mem:
        m['hz']=sorted({round(a[1],1) for a,b in hz if abs((a[0]+b[0])/2-m['xc'])<12 for a,b in [(a,b)]})
        m['bottom']=min(min(a[1],b[1]) for a,b in m['ls'])
        lo=[l for l in m['ls'] if min(l[0][1],l[1][1])-m['bottom']<0.5]
        m['bx']=sorted({l[0][0] for l in lo}); m['w']=m['bx'][-1]-m['bx'][0]
        m['gus']=[ (round(a[1],1)) for a,b in hz if abs((a[0]+b[0])/2-m['xc'])<12]
    # thickness of members at bottom + class
    for m in mem:
        cb = m['w']<9.5
        layer='check crossbeam' if cb else 'check v-stiff'
        off=C['thick_cb'] if cb else C['thick_vs']
        out.append(Dim('R',layer,'DIM15',(m['bx'][0],m['bottom']),(m['bx'][-1],m['bottom']),(m['bx'][-1],m['bottom']-off),0,tag='mem-t'))
    # --- plate thickness + gaps
    def neighbor(x,side):
        # side -1: look left of x, +1 look right. returns ('edge',None) or ('mem',m)
        best=None
        for m in mem:
            f=m['bx'][1] if side<0 else m['bx'][0]
            if m['gus']: f=m['xc']+50 if side<0 else m['xc']-50
            dist=abs(x-f)
            if dist<60 and ((side<0 and f<=x+1) or (side>0 and f>=x-1)):
                if best is None or dist<best[0]: best=(dist,m)
        if best: return ('mem',best[1])
        edge=xl if side<0 else xr
        if abs(x-edge)<60: return ('edge',None)
        return (None,None)
    for p in plates:
        nl=neighbor(p['xm0'],-1); nr=neighbor(p['xm1'],+1)
        def plain(n): return n[0]=='mem' and not n[1]['gus']
        if plain(nl) or not plain(nr): end='L' if (plain(nl) or nl[0] in ('edge',)) else 'R'
        else: end='R'
        if plain(nl): end='L'
        elif plain(nr): end='R'
        else: end='L'
        a,b=(p['L'] if end=='L' else p['Rr'])
        nb=nl if end=='L' else nr
        xm=(a[0]+b[0])/2; ym=(a[1]+b[1])/2
        away=(-1 if end=='L' else 1)
        sgn = away if nb[0]=='edge' else -away
        out.append(Dim('A','check h-stiff','DIM15',a,b,(xm+sgn*60,ym),0,tag='pl-t'))
        # gaps
        for e_,n_ in (('L',nl),('R',nr)):
            if n_[0]=='edge' and e_=='R': continue
            if n_[0] is None: continue
            xe=p['xm0'] if e_=='L' else p['xm1']; ye=p['ym0'] if e_=='L' else p['ym1']
            if n_[0]=='edge':
                pe=(xl,row[0][0][1]); pp=(xe,ye)
                out.append(Dim('R','check h-stiff','DIM15',pp,pe,(xe,ye+62.8),0,tag='gap'))
            else:
                m=n_[1]
                if m['gus']:
                    gy=max(m['gus']); gy=max(y for y in m['gus'] if any(abs(y-z-10)<0.1 for z in m['gus']))
                    gx=m['xc']+(50 if e_=='L' else -50)
                    q=(gx,gy); pp=(xe,ye)
                    pts2=sorted([q,pp]); out.append(Dim('R','check h-stiff','DIM15',pts2[0],pts2[1],(pts2[0][0],gy-49.0),0,tag='gap'))
                else:
                    fx=m['bx'][1] if e_=='L' else m['bx'][0]
                    ry=interp(row_poly(row),fx)[1]
                    q=(fx,ry); pp=(xe,ry)
                    pts2=sorted([q,pp]); out.append(Dim('R','check h-stiff','DIM15',pts2[0],pts2[1],(pts2[0][0],ry+44.5),0,tag='gap'))
    # plain width-9 member chain on h-stiff row
    for m in mem:
        if m['w']<9.5 and not m['gus']:
            x=m['xc']; t=interp(top,x); b=interp(bot,x); h=interp(row_poly(row),x)
            for a_,b_ in ((t,h),(h,b)):
                out.append(Dim('R','check h-stiff','DIM15',a_,b_,(x+C['vchain_off'],b_[1]),90,tag='vchain'))
    # crossbeam member
    for m in mem:
        if m['gus']:
            ys=sorted(set(m['gus'])); pairs=[(y,y2) for y in ys for y2 in ys if abs(y2-y-10)<0.1]
            top_y=max(max(l[0][1],l[1][1]) for l in m['ls'])
            chain_y=[top_y]+[v for pr in sorted(pairs,reverse=True) for v in (pr[1],pr[0])]+[m['bottom']]
            xr_=m['xc']+50
            for a_,b_ in zip(chain_y,chain_y[1:]):
                out.append(Dim('R','check crossbeam','DIM15',(xr_,a_),(xr_,b_),(xr_+C['cb_off'],b_),90,tag='cb-v'))
            lo_pair=min(pairs); hi_pair=max(pairs)
            for y,sg in ((lo_pair[1],+1),(hi_pair[0],-1)):
                out.append(Dim('R','check crossbeam','DIM15',(m['xc']+50,y),(m['xc']-50,y),(m['xc']+50,y+sg*C['plate_w_in']),0,tag='cb-w'))
    # corners
    sh=[(a,b) for a,b in mrk if abs(math.hypot(b[0]-a[0],b[1]-a[1])-100)<1 and abs(b[0]-a[0])<10]
    for a,b in sh:
        topish=max(a[1],b[1])>(sum(p[1] for p in top)/len(top)-300)
        pt=max((a,b),key=lambda p:p[1]) if topish else min((a,b),key=lambda p:p[1])
        left=pt[0]<(xl+xr)/2
        if topish:
            c=top[0] if left else top[-1]
            p1,p2=(c,pt) if left else (pt,c)
            out.append(Dim('A','check h-stiff','DIM15',p1,p2,((p1[0]+p2[0])/2,max(p1[1],p2[1])+C['corner_off']),0,tag='corner'))
        else:
            c=bot[0] if left else bot[-1]
            seg=(bot[0],bot[1]) if left else (bot[-2],bot[-1])
            ang=math.degrees(math.atan2(seg[1][1]-seg[0][1],seg[1][0]-seg[0][0]))+(0 if left else 180)
            p1,p2=(c,pt) if left else (pt,c)
            out.append(Dim('R','check h-stiff','DIM15',p1,p2,((p1[0]+p2[0])/2,min(p1[1],p2[1])-C['corner_off']),ang,tag='corner'))
    return out,plates,mem,row
def row_poly(row):
    pts=[row[0][0]]+[l[1] for l in row]; return pts
