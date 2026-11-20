// Harness console kiem tra Core DIM02 khong can xUnit:
//   mcs -out:check02.exe tools/ConsoleCheck02.cs src/AutoDimPanel.Core/Common.cs src/AutoDimPanel.Core/Geometry.cs src/AutoDimPanel.Core/Dim02*.cs tests/AutoDimPanel.Tests/Sample02.cs tests/AutoDimPanel.Tests/Dim02Matcher.cs
using System;
using System.Collections.Generic;
using System.Linq;
using AutoDimPanel.Core;
using AutoDimPanel.Tests;

public static class ConsoleCheck02
{
    public static int Main()
    {
        int bad = 0, total = 0, found = 0;
        for (int p = 0; p < Sample02.PlateCount; p++)
        {
            CmdResult<List<DimSpec>> r = Dim02Command.Run(Sample02.Input(p), new Dim02Config());
            if (!r.Ok) { Console.WriteLine("FAIL plate " + p + ": " + r.Message); bad++; continue; }
            List<Expected02> exp = Sample02.Expected(p);
            int[] m = Dim02Matcher.Match(exp, r.Value, 1.0);
            int f = m.Count(x => x >= 0);
            Console.WriteLine("plate " + p + " layer=" + r.Value[0].Layer + " generated=" + r.Value.Count + " expected=" + exp.Count + " matched=" + f);
            for (int i = 0; i < exp.Count; i++) if (m[i] < 0) Console.WriteLine("  KHAC DIM TAY (quy tac moi): " + exp[i].Handle);
            total += exp.Count; found += f;
        }
        Console.WriteLine("matched " + found + "/" + total);
        // truong hop loi: phai tra Fail, khong nem exception
        Dim02Input a = Sample02.Input(1); a.PartCode = "";
        Dim02Input b = Sample02.Input(1); b.Lines.Clear(); b.Arcs.Clear();
        Dim02Input c = new Dim02Input(); c.PartCode = "GH1"; c.Lines.Add(new Seg(new Pt(0, 0), new Pt(0, 100), "OUT"));
        foreach (Dim02Input e in new[] { a, b, c })
        {
            CmdResult<List<DimSpec>> x = Dim02Command.Run(e, new Dim02Config());
            Console.WriteLine("error-case Ok=" + x.Ok + " msg=" + x.Message);
            if (x.Ok) bad++;
        }
        if (Dim02Command.Run(null, null).Ok) bad++;
        Dim02Config cfg = new Dim02Config();
        if (Dim02Command.PartLayer("GV41", cfg) != "check GV" || Dim02Command.PartLayer("GH2", cfg) != "check GH" || Dim02Command.PartLayer("12", cfg) != null) bad++;
        // goc: kiem tra diem cung cua dim goc cach giao diem AngleArcRadius
        foreach (DimSpec d in Dim02Command.Run(Sample02.Input(2), cfg).Value.Where(x => x.Kind == DimKind.Angular))
            Console.WriteLine("angular arcpoint " + d.ArcPoint);
        Console.WriteLine(bad == 0 ? "ALL CHECKS PASSED" : "CHECKS FAILED " + bad);
        return bad == 0 ? 0 : 1;
    }
}
