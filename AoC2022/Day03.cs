namespace AoC2022;

public class Day03 : DayBase
{
    public Day03() : base("03") { }

    private (ulong CompartmentA, ulong CompartmentB)[] Sacks = null!;

    [Benchmark]
    public override void ParseData()
    {
        var chars = Contents;
        var sacks = new List<(ulong CompartmentA, ulong CompartmentB)>();

        // My input file contains between 16 and 16+32 characters for each line, and Vector256<byte>.Count == 32
        int vecEnd = chars.Length - Vector256<byte>.Count - 16;
        int i = 0;
        while (i <= vecEnd)
        {
            int end = i + 16;
            var charVec = Vector256.LoadUnsafe(ref Unsafe.AsRef(in chars[end]));
            var isNewlineVec = Vector256.Equals(charVec, Vector256.Create((byte)'\n'));
            var isNewlineBits = isNewlineVec.ExtractMostSignificantBits();
            end += BitOperations.TrailingZeroCount(isNewlineBits);

            int length = (end - i) >> 1;
            ulong compartmentA = 0;

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentA |= (1ul << index);
                ++i;
            }

            ulong compartmentB = 0;
            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentB |= (1ul << index);
                ++i;
            }

            sacks.Add((compartmentA, compartmentB));
            ++i;
        }

        // Attempting to use Vector128 made no difference to performance
        
        while (i < chars.Length)
        {
            int end = i + 16;
            while (chars[end] != '\n')
            {
                ++end;
            }

            int length = (end - i) >> 1;
            ulong compartmentA = 0;
            ulong compartmentB = 0;

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentA |= (1ul << index);
                ++i;
            }

            end = i + length;
            while (i < end)
            {
                var c = chars[i];
                int index = (c > 'Z' ? c - 'a' : c - 'A' + 26);
                compartmentB |= (1ul << index);
                ++i;
            }

            sacks.Add((compartmentA, compartmentB));
            ++i;
        }

        Sacks = sacks.ToArray();
    }

    [Benchmark]
    public override string Solve1()
    {
        int sum = 0;
        foreach (var (a, b) in Sacks)
        {
            var common = a & b;
            int value = BitOperations.TrailingZeroCount(common);
            sum += value + 1;
        }

        return sum.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        int sum = 0;

        for (int i = 0; i < Sacks.Length; i += 3)
        {
            var (a, b) = Sacks[i];
            var set = a | b;

            (a, b) = Sacks[i + 1];
            set &= a | b;

            (a, b) = Sacks[i + 2];
            set &= a | b;

            int value = BitOperations.TrailingZeroCount(set);
            sum += value + 1;
        }

        return sum.ToString();
    }

    private static ReadOnlySpan<byte> Contents => """
        rNZNWvMZZmDDmwqNdZrWTqhJMhhgzggBhzBJBchQzzJJ
        pHlSVbVbFHgHBzzhQHqg
        nVsqGpbbtDtTNmrmfZ
        zrBMnbzBchshsttfbMRBgmJggmmCHGgDhDgNDGHL
        VddZqQqdvSQMJHJGdCDCDDmH
        pZWWllPQlPZQvZvwpSVlqlvtfswMRzBbntzRbzbfstsRzF
        NnjjRlnWNSWWbGwccbcchfPfTvfjfTBBpvmdMjTfvB
        FVzJtDDJDqTMlmlM
        gVQZlFLlzHhLGShGww
        rPZtvtFrFPgWjQvCBlcqMzlqQC
        QGVDJJnLnVTCJBczqqTM
        fNSSnmLDSVLhhhSNSLhGSGfVPjrFHwmQwtwWFRWRjWPHrwgt
        SvmlrVrCvmNhSSVZVCrsgqPfbwGFwwwsflbbGb
        QHffdnHDDQdMGbgqPwztdPds
        DjBjWHfQDfTQWTBfpMBQLVmmmcCCcVhCBBBhhCmC
        trLHFFQHTLHJQrflfCnLLHrRfRRPqSRPbPbbsRGqqGqhjj
        mcMpNWVVNmNVsSbSJPcGhPRR
        NpzNgwzZDVNZVWNpHJQLQHtQrZQHrBCl
        JVCMfgJVrJtMBhhrfVVfhVsjvpFGFgjSSgFdSGGqjvjvqF
        mHllHlHpmWlDSFqbdSTS
        nmZRLzQnWVpctMVpQs
        BrvRzWBPWbRwGRjbbRGrtrfqjCJCjCJgJsZJscFCZcJC
        MnnnVMVhTMQhsccVfwqFJgqf
        mMShHHppQmHrrBzwtSbWwR
        pWWGJMJJwlnZSqjWmvSWZC
        gtHrLttDtgFjjqRZZCrjpp
        bFtbTpHFHLbfLFbHVttccttddJGQdJzTwdTzJlMnMBwwJJ
        JhqHFhVMzJPQcdcVncdc
        NhgfwSjwCWwltSfnrnRWZdpcPrrRnp
        NNhlltBjssNBgwLFFvDmDqLzHqBB
        LnFrnddfrLnMFjWzpFhcWpjpFc
        ntCwgtNggCqCgCqqPPltvcjjhvmWhmvDzTzDzD
        lqlVQgVCSPVllVQSNGMHHrdQsHrJJBnMHHJf
        ZGZcRZNWpcHZhJfbbNblrfrgllNr
        stBMtzCCsHMfFQjfSSPgtt
        qmszdsCzMncdGwdWZGvH
        PccqPqbhvSvvvtWNjTtWsWcscp
        gRwdDzHJQgHzfdRhgHRffzwsTTjTTCjNjssCpmWWDjtCLW
        zdRMwdRHhGJwgHlnGGSFvvSrnSrr
        rRpMJtPwrcCTNNQNMZQm
        mDWdWVddbbbmBflFhvTHjjQjfZTgZgLLfH
        bhBbFFnDVhdddFBhdmpJRrzStJmwnPzcsJ
        RjlpRRWzzRGRmGzlCRRlQjCgtvTJTtJrTPttrWTwhFvvVJFT
        bSBdLLqbcqcLndLHZNqcZdBDPrVTDDTJSFrJJvVthTwwDS
        cqVsnBfHffVdqnZccGMmCsGzQmjsjlljgz
        wMzJhLtwbnMWtHcFCCFqFNNbgq
        fMlMfjrRRmdmGCGVVCHcVqcVTC
        MmRRRlvmQWzpvnZpwJ
        gRmgMRMmRwzzmwHbwcTNqPDVBbPTZVqPNZ
        fWHphpGFpfJrrhPsNTNZVsNVhT
        WGfJdvltJJfHrJpRgvMRMSwRznwMmw
        htJFGsGspCppCFCGthCdpmJmgmWZfqqzWzlWcfgZHgzHlg
        nwVMjVcVcWlbnBlfWB
        wcNDTvPPDMFJLLppDGDD
        hjCBgPbvMvmQDzlWnWjm
        HrHtgZRRRNwczDWwwDzsQQWW
        LpTqNtFtLFqHLHRrqgFHffVVBChvhhVPBCPhbPbp
        CwpbCwjGqSjVllpGCllBfhZZRDPNcPPNvLLLDSDN
        WshFFWsgTHsdMzQvPczLfLZDZRcLfR
        rWsJQTMhWWHdsQTgsFJgllClVpqVbqnGblCppCVr
        gRBSGcBDBSJSvPQwrTFLjggQTQ
        HMMnHHHZfFVFrrMT
        HhlhppCNcJzCTtBT
        CCffCCmRLTsQRPHQQMPF
        dWdbgcDSNclbbdwdSqHsvHPQPTPJplPMFMGJ
        DWbDNcqZDSWSccNTVBCzVVfmBVZnVz
        BnsrrvZwBsBSJrrrqSTgJQjCbCjgbCHDJgJFjQ
        hLmGlnLmGWcjGDgfFFjQdF
        hhWPmhPtczWpNRmppzRhLchMsnwZvTMZvVSwwrsNwSsBvr
        tDCCltNVttJhNGlMPSWdqBqSjM
        RFQcpcRTpFcnFzdLmLSWjMSSBLSQ
        jwzzczpFbwnHcDCsthDJJsNbst
        dLRWTHSwTmTwTcTWvQNVVQCvVvNFps
        GnBPtBMJBPrjGGJMjrlqChNpNlsnhVFhQsVQ
        JtMtGJfrJgDJjPjRTZLdFcRZRmwSDH
        VSccPJSBLgZPDLDQ
        zfpLMmLsHQGqgQHnDD
        zdLLMssmrdfhddcVdJtScB
        VvpTVQHSqSHSHqqHJVmRJVHpgDBwDgjcDDDgZjBZBjwBZbRw
        PCdssGlstdWslFPfNPrtClGjwBgBJgJNwcjBjBgZwwMBJD
        tlJldhdhdsdhTqSTqVQqQq
        VGqTcTqbpPwrjfbl
        BvntnZNNsLZvLszSnCsvJthlfjTrZwlrjrpPlwlhfwrl
        QBtNtJLvTsFdQcqWmQRR
        fjcjhmjBvcvcSvcZ
        HMwZtRQQpGGRgzMvLnWWnbLlSntlbv
        JQPzzJHqQRqGMMQwHwzDZZhmmPfjDjmjsCZhPj
        cBlZZMfBrCBMwBMCvQzTwFbQzPnbwjTbTg
        WtzpVDzmtthzGFQTbTThnnTQQg
        sGWstpHdpGDmdHdmGmmmJNstRMrCcBSfBSzNBNRrSRNMcMMv
        mMPDVBZZLSmRdcFpjr
        fggGGfbfgQStjjsdbtdt
        gNqQgCQlNCCJgJHvnvnHMjPHjv
        bLsRQrQsGQbLrbRZMGgbJJBJFtlFFngJphhcfBBq
        jjdHCCjfVNmmmNDFcBcpBthcplFDFq
        jmvvmWVjjHTCVvNjSbQGLrRzwMWsMRwfGG
        sJNCsCFFCNPhCzlrSvRrvwhRjj
        MMGMTwpMHGzrGczzlG
        qVmwgHtDtmCdWCsNFmNJ
        fmhWhjVjNpqRRJjwRw
        gnGQGDDCgSsCvPlvPgnPgnPtwqbpHRHqHdJpzpQJJJRJRF
        wgPGsDGPsZgGgBmBWNZNfLWWrZ
        WdsCVtjWWWHRRqLLHncC
        fbSpMSPSZHRRcqlpRc
        cGMmJmfMPPPccZMNQPWvjTtdTjvgmdtTsggw
        tPBQhHWBtQHgWQCtLwddcGnfpGpwwnbhVb
        vqQzTNJJJTvRrTNFJsZrrzFlbbfcnVbbcwmGGGpVzmddcdfd
        NSSqJvFFFFFQjQCjQDSDPD
        rQZnVVrZmZmgSWqHrSzHPC
        LGFLwcMBcllBjFNwGjltggSqSWCCzvNgSqSHtt
        wdhqqGBwwqGMcDhcwdFFbbJppZbssbfZQsQsdVQm
        lqBZlsjVTbVqmFrSnTFSvwncPP
        zQztHfZQtWLJzPFnnQScFcFrvS
        ftHJWHhfttHWffhtgLNfZDWbdqBqjbVssBDCqCdCsmClGG
        MlbWFTJQFbFFzRdNjNtjdtBT
        srwnrsLVHzQPQsjjSQ
        gLpnwgnwnHCvcHHcvwgCvGFFhWGmFmqMMbQFQFFhlGmJ
        qqNcJgJccdqhsqgsggdgqgcrtfNWNZzVbvVFzttMfzbVMZ
        GLlpPpCpwPLDGvrFVWrWWbZt
        DlRCDDLSjTjDjSRSjPClwnwSHHHQmmQvTJcQgvddHsqdcgmB
        jmRjRbRQLLZbPnbrcTTHHHNn
        MfhhmmwtvStrpnJJHc
        fgqlvfhvFzMwqfvMfFWlmMvLZsdQsZVdCdLZdGQjRzdQjD
        lTPcDlVdTlVVMSDfTJccVzdlmMgGBmppgBmnHGHqHqQqqQMH
        ZRjWFPsLNLLrPhWNtnBBvnpGpHGpQmHnmR
        CtwssCNLrsZWjrjcbfPzwJJJffDbTl
        cjMvvqpJFqhShNCRQR
        ldtDgQZDPdzztLZgPTtfbnStfBSbNNSbnbhhSS
        TDsrzsZZZTFHmVHjcsQW
        BQmQchrmBddcmZZdpSgrpswWWswVsnnnDJVnnZFnGN
        TfStMPLTHvbvRVGnHGsNnJWFNV
        qtvMRMMPbbPMLqRPvRTRzMjSSmprpQdBchlmmgldgjzm
        nRRnvNPhrbZDLjvS
        HCszMwcHHcLDrbQDWr
        ptszqwdMbnnhPBqN
        QbzhhfbFhBbpbzwwLjLJjSjltL
        mNndGrSStHJTJLln
        rDMMNVWdVpCbSbSp
        tDTSTSTTTTJDwqjWqBWttdjg
        nNPmVfnGfPNVLmNzfnzPVFMjdpBwWZwZHwBLBqgjqpWH
        dfGPfVQGVPhGzlmnzSvsSTDJhTbTTrrSRD
        ZfgtZBptBfRQNQggjjrjjwmwsQJPzrwm
        TwTGGwTwzzsJzTsH
        lFvwqFLhFMnqcLlVLMLfptNWppppDBDbDfbFgW
        mjftBfVPjttmjcSjcPttzJlvnrwvTRrTnvwvlRrHHTHRTR
        WZDWDNLFWbZbcMDWGZDbNdMCRsnTdTvdnqrHCTrvsRRvwC
        DQFZLNNgtBJQcBzJ
        HbZQZFVbQVpQplQZGbGchDffltfLtmdgDjggTmtm
        zWzRCdnCRBRdJrzDjLhDthjLJTTtjq
        CPPnwSrRdRSzCGMcZZZMwFwMZF
        WBQqNQnQllwnWQlvBBMlljHTqqFdGfmTdFfcFTFFcqmP
        rsRRVrZhrzbtpZRRhFDmPvfFFrfTdFHGvc
        VtSCtSLbtsZVtttthCbJSWSlJlwJQggWWglvwW
        QfFLWCvRfSLFCtvtFhNcqDDcGVbhGcqh
        ZVgrdZZPPZZzPwdjzZhmccsqJGqDdsDDNddD
        pzzwpgZzZZTznZnjZZzPVRLQLlvfSlQRSpWlCvtSQv
        RtcHhRMcrHhBrrTNDVBNLqLqQqfBPm
        wCbWzWbvdWCjbWppmtmNmqmLLsfsNV
        lwjWdbztgHTgggnnnR
        flBbzbMfbrTlrMvBCcwPggdmcdmg
        VDVVRFZRZSFFhQLSGFQhjSVZCgpvPwLCzpdWWzccwdvvvwcC
        hDHRGQVHHQVRZSQGbqqfNTlbHzrbbsqb
        MTFdTsZpPTcMpFCPdCBmMBmRfRGBmQgQRRgt
        vbDSwvhzznnbbhDWnvSzRBgQQLgLQltqtqlmwfGB
        jVjhfSnNDNbzzWzjWSjrCFNpcHdpTTJddJFpsJcc
        ZrrZPHfChPdDPVVdDq
        vFmsbTsmSbbBJssmSBvTmmnTrnrwlWqwVlLrVTLLTWqL
        JrFbpsvFBMBmzBzFStcRhjZjfCCpZNCtct
        TGgRrTggwwtvtQtdCdQNqN
        sJHZJVZHDBpFBZBBNzNdhzdpSzddvqhN
        VZcvFsJVFvsmvssbcnrwbrnGMbMlRn
        SdcdWzMJdSMWMddZJdVcmBmwrwqrrnVnVNtr
        mlQHCfgbjsfQTbfCBNtVhVnntVBnVh
        HLDslDDmblgHfvLHPJFSZPpDFpFFpdPS
        qNqPNJvcSzGGPQnGQp
        bWhbgsshZWBhltthhbWtCsZNjrzpnQnnznnjtQFrjGjVFGnn
        bRDNddhNdDsZdNChmvDmmwqqvLqwSJDq
        TnSfPnCSmnSgpSTmfLzfMFLWFJJLWWsBsr
        jdQjcdqDVVwDcPsPzMRJMLqPqR
        PGhGchjhtZlTGTHCCb
        ZZRrJJqSqJwNFFphsGsLPJ
        blcMCflvTTPFFNpVvsFv
        CcTlltTmtmMdmCmnlllBDDSDQSwSjRDQSdswjR
        MCCPNsnQFWbvvTPF
        CcCVJJhjVJZRtcCclDDlbcbTcGFFDz
        HpjtVwVZfpjJVhZgCVtLmrBwdMrLsNNsMmdLqB
        TJTDTnrFzzdWgWGJSSMJwg
        LhPVttjtLmsPqqqVsVpsjLlgWlwHvGnlHWlgHlGgwvlP
        mQshLhmsnsqZcqhZqpshsLVpNTNbBfzTRBQdFRzNNFBTdbzR
        ZGqMLGqvJsJsMJmd
        PDVQPfPcrrcFrrzrTdgCjSSCzgszmlJjBj
        PfRtVfttVcWtVJrfbGqvwqLpRRwvpppH
        HmLmMSnnWnrTrnvpqFCHVGfzVFVHQj
        ttsstRhhcNwbswNtdwsdNPFfjzQppQPjfGGfQVPCpR
        bbsDNtDcbhstsSZLDmSSgCmnSS
        tfwBBLcJVrDnqvLv
        zmWWJRZhWRRRGRNdgSZGgWTvpnjvrDqvpHjjzrpnrPDnHj
        NdJmSGZWRhRNsghWTJmdGfQCtllCcFMwffBftsfMQc
        lTLgTghpGZJDBrnGWnnm
        VlRwlHttwqmHHbDWHJ
        twldzCvsRdsFFtRtSczTjSgMcfSpSzTM
        pBpMBTcSlNtMcTfFCmbPDzCDLb
        JgrjjJqhGZQrQrZhnJGDDCZfvPDdDzFFdzfmZL
        QHhqqnrVJJPhHrnGQgwMNwMMctcWRWSBMNtNsW
        FJrlhpcfDCcFWpNpwWwjNQwz
        RTTvPdbjWzMbnNNM
        GRZTGggGgtvjGcqrBcttcDlFhr
        pMRVdVbbMMMSdWWqHpCTvTjnBBBFFGGB
        smNfZgcsNrcmzggZszsgRnPGFHjBPTBTjGjPTBNj
        RmwgsmgfrzzsZtfgZLQQSVWlwbdMhlwdqQ
        mRRjPmLrrSmzSczSzPgVZFpTCpZCMWrZQMQrZJZT
        BvdbHNdnJtvBDbqqdBlvwvqpDQMpZQFMCsQCspZTMMCZCF
        nBlfbfbndJBHPfLRfmhhhhPL
        ScJDFBNLLbVRqVfZ
        rWrgmdMgnnBhBtnntf
        CwBWWMgCwddCgwsQjsrvNvlTJzSNHwNTHFJHzS
        vnddCrNpCgtjLdSdgCgCCvLnWqDhWBQhHqQHDqBhQHDHNNDl
        wPTVfVTJmZGJVJGffZBwHMWlWlHlWtbQDqbl
        mGsJVVJsTVTTmtJVzzTJjdSjjprzCvpSLSCjdnLg
        zLNggsVHmNNsssLmwzLQZLwDRvGQBqGGDDBBvvDBDqPhRG
        WrCjbtJdbFhBRglGgjqv
        JWCJcWcSdWcctnJCcJJJbcbmzwwznmgLzNzmLHmHZMwsZL
        JRRDNNhhszMTzNMwCG
        MnHPqmgmHjPnnvjqdmjFLQwLwTLwzTwTdGLCzS
        BnPPZqmcfqgqnnZmBmqjqhfWVJlRMlhWlRDlVsssbh
        nmTLTqsvqnwqsvwDPnLHdNVrMMHHCBlmVdmGNV
        RgRpcJhQRfQZcJbWhQpBHCjVCdjCVGdddMllHp
        fczbZhzbtcZfgRRBcWSPPwFsLSDswSwTsSzw
        rbFpzFCVBrrBZCjbCzHHBVdJllGDLsLrDtsswswstGJs
        QNhNNnNnnQhNWSnRhnJtdpJpJtMDGsGLLtsQ
        ScmRvNRNnWWvNvNvfpTccjVZbqgZgVzqHjCjTVTVVq
        BTppwCwBpwwBqnjlHcLBTHnbbSbDthsSSJgsnDDRgJRD
        FVGzzvrdMGSSsdtZtZgd
        QvQtvtGFlBLLjLQL
        gsWWsNMjwgPMPWnMjShHHZSZbmZbbmTSnb
        rlCvVQrCfqffpVjQRqCCvDDTTTmmZhZTmZhThFmhhZZhqb
        CDDVJpVfrJJVJLMNzMwWwLwj
        nHrcsZrssPcBPtQJLJtQQCZQpV
        GFWzNzNFdNbTMMqbGTqTqzqqdLCpfDQCtRVVCLtdCfQsdCCt
        TlNqGTWFNmMMszhGsmFTWGFzwHnvSjgPgvgSjllBvBnvwPBB
        mpMggjgMlmtjtGMwZpcSscBlcsSblhsfSdfs
        zzPVDRrLrCTQNCzNRTVFNLhBhBSqdQbcfSsJBJdbjJfB
        RPTRPTVNTFzVrHVDCrTHmHtwMvwWMmtwmGjWgvGv
        rLMcvfHVfMgLFvfNnBBzwRbBwnrGNs
        dttJjJCtdjmwzwBCRRCqcs
        TddDQDJDtQJtcJFpPQHPQMvfQlFL
        LQSqqpqTCSJcsDcqQMMhnnjMjppZhwHZbZ
        NRtvtmgmvdBffgtVCBWVRgFbPzHbMHbnwwjMPZfHbPjzPP
        RNtvCvNdgtNNmldgvCFRNVLsQLqJcQGJJrccGSlDLDLr
        GdwwqqqwGVtjdPvTCplbHTPbPzPTpp
        RpLmLLpFfNsgTzclhzClThgH
        ZFsWZLFZJsNsnWsnRsRfnfJQGBttjdGJjBvvwjdpjjttvj
        tfPzzLrrdrQlTlvn
        qJRBhNhNGVRBFRTlnJvCmvmJPCCl
        VVPDNchNMVFGRMFcRVBjsZZcttSLSZzzStcWtZ
        pTrwTrnjtttjprTSTNTQfcjcgPsPZfPgjdgdsQ
        mCmCzvzhmJDHzJDbhFCDPsgddcsfcdsbdgVRpdVs
        zqJzFCDhmqvGhMmCvmGhMCGJnSlnllSBLllLMtNpWtpNBnlt
        JBhJrFLhGrnJZrlcbffndnggfggf
        jqmWMGGSsqCsmpjmsDQzlcHgbtdzjjlVfctjHV
        GWSmSCspCsMSpRmSmqMMCBvFLJLhTTwFhRFLLBTwrv
        BCdWccqcqpQqrsNgGsWMgfNW
        lFttLzzLwnfsLrsNsNLG
        zjNlznlwvRPZnltwvPFnZRCbmjCcqjpcpQcqVVdbdVBm
        CwTbbCGNFHtHwwjSjJpzjLMdMMzT
        rscqqVvWgWrZMjrlmSzzmLrM
        WPqqZnPqgncnBQQVRbCDwRHGSFHPwRNw
        ZQnZwWjFvdsHwBJltfmfSlsqlJ
        gPprhMDTpMpPMVNqNRqNlJhltJdJ
        pLGCcCrgppCrVcMpdzjvzvjLwQQzFjwzHF
        NmmmvfqcvmLSQhCLvtvL
        TVlWTZVJZJsFbwWbQQhtQgLFCnSgghLt
        hZJTJZhwZlRJrJWHVlblMBffmqfdNMjdGdBBqqcH
        GJJfLfptGqqqnsVqVVjjDnNc
        mZPSvPmBCdmwdCLDshSbRnnDDhRL
        gvBrBvPBPPZCTLZmwmrgQdwfTJMHGzHfWffJzFzttHWFzW
        sBMvmzWzmFmNWJfffZNLfbqZbtZq
        jRQVRnhhppnVhjgnDLttLqbLqLQfDLss
        jRRgpGVGhwhnspgpRppwSnBvMMcWvGczGJJHdmHJmJFF
        VCLHFwHMhLghHHWhFFgWNMMVzmdmbvWdJqBPJPPBppqmBdzm
        SRTsjGZTsZZnSnGZGqdBmrqPvmqqqsPpmv
        GvQSGtZSQllVhtLMcLLNMH
        GsNdWpdVWGSHjFCWCqFFgqngvW
        mRQTcrLRmZTPRLPZfqqqHbDDDgFvFnvqzQ
        hfZHrwwmcZRwlLfwlmrRjMJJsVjslVNBGNjpVBBG
        pllpztRqBBvvGPpG
        QQhhZQbVcZQTPMWWGbvvbMHM
        cwgCQCLZChQwwLZVzCrzzqNCzrDqdFPF
        bgcLPvvpcbdsbpSsHRTCqsRfWfsHRm
        lZlQtthrnlVMmTHqqqqHSChB
        rDtlzttnlSNrMtQjZVrcgGDLLddcdcpPgPGJJd
        jvGbvLLQDSGlRmmSLjlDmRQggFBrMCwWdsBFWBFjdrrWrr
        PpTfcPZpNTVNpHzTzzzpPJhBcwrrhFsrMdFcMCBFhgMF
        JTTqdtfzfzJpqffNdTTHGtQRnmDQGGLQQlQRbblD
        CQQCshCMwgQhMdjWJFBPpbjgmmWj
        SNNvcGNSZSTDtGDcczJJBmzbjBJjmppbppms
        cDtfDVNTGGGNNrwLLwHdqLhfLs
        ngghZCChzhNjjNbbJfdh
        slPPRLlBBlVRMvRllLLHvcpcdFfJjvdFpfHfcZ
        RDZPZBLmPVWDVrQtnzSTmgTwmTSg

        """u8;
}
