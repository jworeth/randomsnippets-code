using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Flux.WoW.Native;

namespace Flux.WoW
{
    /// <summary>
    /// Kudos to Cypher for most of the shit in this class. 
    /// </summary>
    public static class WoWAuction
    {
        public static uint ListAuctionsPageCount { get { return Reader.Read<uint>((uint) AhAddrs.NumListAuctions); } }
        public static uint OwnerAuctionsPageCount { get { return Reader.Read<uint>((uint) AhAddrs.NumOwnerAuctions); } }
        public static uint BidderAuctionsPageCount { get { return Reader.Read<uint>((uint) AhAddrs.NumBidderAuctions); } }

        public static uint ListAuctionsCount { get { return Reader.Read<uint>((uint) AhAddrs.FullNumListAuctions); } }
        public static uint OwnerAuctionsCount { get { return Reader.Read<uint>((uint) AhAddrs.FullNumOwnerAuctions); } }
        public static uint BidderAuctionsCount { get { return Reader.Read<uint>((uint) AhAddrs.FullNumBidderAuctions); } }

        public static List<AuctionEntry> GetAuctions(AuctionListType type)
        {
            var ret = new List<AuctionEntry>();
            uint numAuctions = GetAuctionsCount(type);
            IntPtr auctionsPtr;
            switch (type)
            {
                case AuctionListType.ListAuction:
                    auctionsPtr = (IntPtr) AhAddrs.ListAuctions;
                    break;
                case AuctionListType.OwnerAuction:
                    auctionsPtr = (IntPtr) AhAddrs.OwnerAuctions;
                    break;
                case AuctionListType.BidderAuction:
                    auctionsPtr = (IntPtr) AhAddrs.BidderAuctions;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            for (int i = 1; i < numAuctions; i++)
            {
                ret.Add(Reader.ReadStruct<AuctionEntry>(Marshal.ReadIntPtr(auctionsPtr, i * 4)));
            }
            return ret;
        }

        public static uint GetAuctionsCount(AuctionListType type)
        {
            return Reader.Read<uint>((uint) type.GetAddr(false));
        }

        public static uint GetFullAuctionsCount(AuctionListType type)
        {
            return Reader.Read<uint>((uint) type.GetAddr(true));
        }

        public static uint GetPageCount(AuctionListType type)
        {
            uint fullAuctions = GetFullAuctionsCount(type);
            double tmp = fullAuctions > 50 ? fullAuctions / 50 : 1;
            return (uint) Math.Round(tmp, 0, MidpointRounding.AwayFromZero);
        }

        #region Nested type: AhAddrs

        internal enum AhAddrs
        {
            ListAuctions = 0x11FCF84,
            OwnerAuctions = 0x11FCF94,
            BidderAuctions = 0x11FCFA4,
            FullNumListAuctions = 0x11FCF48,
            FullNumOwnerAuctions = 0x011FCF4C,
            FullNumBidderAuctions = 0x11FCF50,
            NumListAuctions = 0x11FCF80,
            NumOwnerAuctions = 0x11FCF90,
            NumBidderAuctions = 0x11FCFA0,
        }

        #endregion

        #region Nested type: AuctionEnchantInfo

        [StructLayout(LayoutKind.Sequential)]
        public struct AuctionEnchantInfo
        {
            public uint Id;
            public uint Duration;
            public uint Charges;
        }

        #endregion

        #region Nested type: AuctionEntry

        [StructLayout(LayoutKind.Sequential)]
        public struct AuctionEntry
        {
            private uint _unk00;
            public uint Id;
            public uint ItemEntry;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public AuctionEnchantInfo[] EnchantInfo;

            public uint RandomPropertyId;
            public uint ItemSuffixFactor;
            public uint Count;
            public uint SpellCharges;

            private uint _unk70;
            private uint _unk74;
            public ulong SellerGuid;
            public uint StartBid;
            public uint MinBidIncrement;
            public uint BuyOut;
            public uint ExpireTime;
            public ulong BidderGuid;
            public uint CurrentBid;
            public uint SaleStatus;

            public override string ToString()
            {
                return
                    string.Format(
                        "Id: {0}, ItemEntry: {1}, EnchantInfo: {2}, RandomPropertyId: {3}, ItemSuffixFactor: {4}, Count: {5}, SpellCharges: {6}, SellerGuid: {7}, StartBid: {8}, MinBidIncrement: {9}, BuyOut: {10}, ExpireTime: {11}, BidderGuid: {12}, CurrentBid: {13}, SaleStatus: {14}",
                        Id, ItemEntry, EnchantInfo, RandomPropertyId, ItemSuffixFactor, Count, SpellCharges, SellerGuid, StartBid,
                        MinBidIncrement, BuyOut, ExpireTime, BidderGuid, CurrentBid, SaleStatus);
            }
        }

        #endregion
    }

    internal static class AuctionListTypeExtensions
    {
        public static WoWAuction.AhAddrs GetAddr(this AuctionListType addr, bool full)
        {
            switch (addr)
            {
                case AuctionListType.ListAuction:
                    return full ? WoWAuction.AhAddrs.FullNumListAuctions : WoWAuction.AhAddrs.NumListAuctions;
                case AuctionListType.OwnerAuction:
                    return full ? WoWAuction.AhAddrs.FullNumOwnerAuctions : WoWAuction.AhAddrs.NumOwnerAuctions;
                case AuctionListType.BidderAuction:
                    return full ? WoWAuction.AhAddrs.FullNumBidderAuctions : WoWAuction.AhAddrs.NumBidderAuctions;
                default:
                    throw new ArgumentOutOfRangeException("addr");
            }
        }
    }

    public enum AuctionListType
    {
        ListAuction,
        OwnerAuction,
        BidderAuction
    }
}