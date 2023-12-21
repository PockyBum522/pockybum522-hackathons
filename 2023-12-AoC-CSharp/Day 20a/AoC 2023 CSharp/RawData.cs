namespace AoC_2023_CSharp;

public static class RawData
{
    public const char FlipFlopModulePrefix = '%';
    public const char ConjunctionModulePrefix = '&';
    public const char BroadcasterModulePrefix = 'b';
    
    public static string SampleData01 => """
                                         broadcaster -> a, b, c
                                         %a -> b
                                         %b -> c
                                         %c -> inv
                                         &inv -> a
                                         """;

    public static string ActualData01 => """
                                         %hf -> qk
                                         %xp -> pn, mq
                                         &rz -> lb
                                         %nm -> hn
                                         %zb -> gz
                                         &lf -> lb
                                         %nn -> vd, zc
                                         %xn -> fz, kb
                                         %gz -> vf
                                         &pn -> lz, hq, lf, mh, bh, mq
                                         %xb -> th
                                         %vf -> nc, vd
                                         %ds -> kz, pn
                                         &br -> lb
                                         %cm -> fm
                                         %qz -> bh, pn
                                         &lb -> rx
                                         %vx -> pn, ds
                                         %kz -> pn
                                         %gp -> cm, th
                                         %hq -> mh
                                         %fq -> xd
                                         %mj -> th, np
                                         %lz -> vq, pn
                                         %hn -> xn, fz
                                         %fl -> fz, rq
                                         %fm -> hh, th
                                         %tx -> fz, qn
                                         %mh -> xp
                                         %dn -> nm, fz
                                         %xv -> vd
                                         &vd -> zc, nn, hf, br, zb, tp, gz
                                         %np -> fq
                                         %sf -> vd, xv
                                         %rq -> fn, fz
                                         %zc -> ms
                                         &fk -> lb
                                         %qn -> fz, bt
                                         %qk -> vd, zb
                                         %ms -> tp, vd
                                         %xd -> th, gp
                                         %hh -> th, dq
                                         %sx -> th, xb
                                         %fn -> fz
                                         %jd -> pn, vx
                                         %mq -> jd
                                         &th -> mj, rz, np, fq, cm
                                         %bt -> fz, dn
                                         %dq -> dj, th
                                         %tp -> hf
                                         %nc -> sf, vd
                                         broadcaster -> nn, lz, mj, tx
                                         %bh -> hq
                                         &fz -> fk, nm, tx
                                         %cr -> fl, fz
                                         %vq -> qz, pn
                                         %dj -> th, sx
                                         %kb -> cr, fz
                                         """;
}