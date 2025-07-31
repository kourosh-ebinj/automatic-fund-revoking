using System;
using System.Collections.Generic;

namespace Core.Enums
{
    public readonly struct CacheKey
    {
        private object Key { get; }

        public CacheKey(object keyValue)
        {
            Key = keyValue;
        }

        public static implicit operator CacheKey(string key)
        {
            return new CacheKey(key);
        }

        public static implicit operator CacheKey(long key)
        {
            return new CacheKey(key);
        }

        public static implicit operator CacheKey(int key)
        {
            return new CacheKey(key);
        }

        public static implicit operator CacheKey(byte key)
        {
            return new CacheKey(key);
        }

        public static implicit operator CacheKey(Enum key)
        {
            return new CacheKey(Convert.ToInt32(key));
        }

        public static implicit operator CacheKey(DateTime key)
        {
            return new CacheKey(key);
        }

        public static implicit operator string(CacheKey key)
        {
            return new CacheKey(key).ToString();
        }

        public override string ToString()
        {
            return Key.ToString();
        }

        //public bool Equals(CacheKey other)
        //{
        //    return Key == other.Key;
        //}

        //public bool Equals(string other)
        //{
        //    return Key.ToString() == other;
        //}

        //public bool Equals(long other)
        //{
        //    return (long)Key == other;
        //}

        //public bool Equals(int other)
        //{
        //    return (int)Key == other;
        //}

        //public bool Equals(byte other)
        //{
        //    return (byte)Key == other;
        //}

        //public override bool Equals(object obj)
        //{
        //    return obj is CacheKey other && Equals(other);
        //}

        //public override int GetHashCode()
        //{
        //    return (Key != null ? Key.GetHashCode() : 0);
        //}

        //public static bool operator ==(string x, CacheKey y) => y.Equals(x);
        //public static bool operator !=(string x, CacheKey y) => !(x == y);

        //public static bool operator ==(long x, CacheKey y) => y.Equals(x);
        //public static bool operator !=(long x, CacheKey y) => !(x == y);

        //public static bool operator ==(int x, CacheKey y) => y.Equals(x);
        //public static bool operator !=(int x, CacheKey y) => !(x == y);

        //public static bool operator ==(byte x, CacheKey y) => y.Equals(x);
        //public static bool operator !=(byte x, CacheKey y) => !(x == y);

        //public TypeCode GetTypeCode()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool ToBoolean(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public byte ToByte(IFormatProvider? provider)
        //{
        //    return (byte)Key;
        //}

        //public char ToChar(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public DateTime ToDateTime(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public decimal ToDecimal(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public double ToDouble(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public short ToInt16(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public int ToInt32(IFormatProvider? provider)
        //{
        //    return (int)Key;
        //}

        //public long ToInt64(IFormatProvider? provider)
        //{
        //    return (long)Key;
        //}

        //public sbyte ToSByte(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public float ToSingle(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public string ToString(IFormatProvider? provider)
        //{
        //    return Key.ToString();
        //}

        //public object ToType(Type conversionType, IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public ushort ToUInt16(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public uint ToUInt32(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public ulong ToUInt64(IFormatProvider? provider)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
