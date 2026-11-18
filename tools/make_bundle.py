"""Gom code thanh mot file de gui kem moi chat.
Dung: python tools/make_bundle.py [--with-samples] > bundle.md
Mac dinh bo qua file du lieu mau sinh tu dong (Sample*.cs) vi rat dai.
"""
import os, sys

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..')
EXT = {'.cs': 'csharp', '.csproj': 'xml', '.props': 'xml', '.py': 'python', '.md': 'markdown', '.sln': ''}
SKIP_DIRS = {'bin', 'obj', '.git', '.vs', '__pycache__'}

def main():
    with_samples = '--with-samples' in sys.argv
    out = ['# AutoDimPanel - ban gom code', '']
    for base, dirs, files in os.walk(ROOT):
        dirs[:] = sorted(d for d in dirs if d not in SKIP_DIRS)
        for f in sorted(files):
            ext = os.path.splitext(f)[1]
            if ext not in EXT: continue
            if f.startswith('Sample') and f.endswith('.cs') and not with_samples: continue
            if f == 'bundle.md': continue
            path = os.path.join(base, f)
            rel = os.path.relpath(path, ROOT).replace('\\', '/')
            with open(path, encoding='utf-8') as fh:
                body = fh.read()
            out.append('## ' + rel)
            out.append('```' + EXT[ext])
            out.append(body.rstrip('\n'))
            out.append('```')
            out.append('')
    sys.stdout.reconfigure(encoding='utf-8')
    print('\n'.join(out))

if __name__ == '__main__':
    main()
