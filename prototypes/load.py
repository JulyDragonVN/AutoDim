import os
import ezdxf
# Thu muc chua DXF: dat bien moi truong DXF_DIR, mac dinh la thu muc hien tai.
U = os.environ.get('DXF_DIR', '.') + '/'
def P(v): return (float(v[0]), float(v[1]))
def load(f):
    d = ezdxf.readfile(U + f)
    return d, d.modelspace()
