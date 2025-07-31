using System;

namespace Core.Helpers
{
    public static class AmountToFarsiEquivalentHelper
    {
        public static string ConvertToHoruf(string s)
        {
            string res;
            res = "";

            int len = s.Length;
            int mod = len % 3;
            //if(mod!=0)
            //    res = ToHoruf(s.Substring(0,mod));
            //else
            //    res = ToHoruf(s.Substring(0,3));
            if (len == 1 || len == 2 || len == 3)
            {
                res = ToHoruf(s);
            }
            else if (len == 4 || len == 5 || len == 6)
            {
                if (mod != 0)
                {
                    res = ToHoruf(s.Substring(0, mod));
                    s = s.Substring(mod);
                    len = s.Length;
                }
                else
                {
                    res = ToHoruf(s.Substring(0, 3));
                    s = s.Substring(3);
                    len = s.Length;
                }

                res += " هزار";

                if (Convert.ToInt64(s) > 0)
                    res += " و " + ToHoruf(s);

            }
            else if (len == 7 || len == 8 || len == 9)
            {
                if (mod != 0)
                {
                    res = ToHoruf(s.Substring(0, mod));
                    s = s.Substring(mod);
                    len = s.Length;
                }
                else
                {
                    res = ToHoruf(s.Substring(0, 3));
                    s = s.Substring(3);
                    len = s.Length;
                }

                res += " ميليون";

                if (Convert.ToInt64(s.Substring(0, 3)) > 0)
                    res += " و " + ToHoruf(s.Substring(0, 3)) + " هزار";

                s = s.Substring(3);

                if (Convert.ToInt64(s) > 0)
                    res += " و " + ToHoruf(s);
            }
            else if (len == 10 || len == 11 || len == 12)
            {
                if (mod != 0)
                {
                    res = ToHoruf(s.Substring(0, mod));
                    s = s.Substring(mod);
                    len = s.Length;
                }
                else
                {
                    res = ToHoruf(s.Substring(0, 3));
                    s = s.Substring(3);
                    len = s.Length;
                }

                res += " ميليارد";

                if (Convert.ToInt64(s.Substring(0, 3)) > 0)
                    res += " و " + ToHoruf(s.Substring(0, 3)) + " ميليون";

                s = s.Substring(3);

                if (Convert.ToInt64(s.Substring(0, 3)) > 0)
                    res += " و " + ToHoruf(s.Substring(0, 3)) + " هزار";

                s = s.Substring(3);

                if (Convert.ToInt64(s) > 0)
                    res += " و " + ToHoruf(s);
            }
            else if (len == 13 || len == 14 || len == 15)
            {
                if (mod != 0)
                {
                    res = ToHoruf(s.Substring(0, mod));
                    s = s.Substring(mod);
                    len = s.Length;
                }
                else
                {
                    res = ToHoruf(s.Substring(0, 3));
                    s = s.Substring(3);
                    len = s.Length;
                }

                res += " همت";

                if (Convert.ToInt64(s.Substring(0, 3)) > 0)
                    res += " و " + ToHoruf(s.Substring(0, 3)) + " ميليارد";

                s = s.Substring(3);

                if (Convert.ToInt64(s.Substring(0, 3)) > 0)
                    res += " و " + ToHoruf(s.Substring(0, 3)) + " ميليون";

                s = s.Substring(3);

                if (Convert.ToInt64(s) > 0)
                    res += " و " + ToHoruf(s);
            }
            return res;
        }

        private static string ToHoruf(string str)
        {
            string res = "";
            char temp;
            long digit = Convert.ToInt64(str);
            str = Convert.ToString(digit);
            int len = str.Length;
            int mod = len % 3;



            if (mod == 0)
            {
                temp = Convert.ToChar(str.Substring(0, 1));
                switch (temp)
                {
                    case '0':
                        break;
                    case '1':
                        res = "صد";
                        break;
                    case '2':
                        res = "دويست";
                        break;
                    case '3':
                        res = "سيصد";
                        break;
                    case '4':
                        res = "چهارصد";
                        break;
                    case '5':
                        res = "پانصد";
                        break;
                    case '6':
                        res = "ششصد";
                        break;
                    case '7':
                        res = "هفتصد";
                        break;
                    case '8':
                        res = "هشتصد";
                        break;
                    case '9':
                        res = "نهصد";
                        break;
                }




                str = str.Substring(1);
                digit = Convert.ToInt64(str);
                if (digit <= 20)
                {
                    if (str.Substring(0, 1) == "0")
                    {
                        temp = Convert.ToChar(str.Substring(1, 1));
                        switch (temp)
                        {
                            case '0':
                                break;
                            case '1':
                                res += " و " + "يك";
                                break;
                            case '2':
                                res += " و " + "دو";
                                break;
                            case '3':
                                res += " و " + "سه";
                                break;
                            case '4':
                                res += " و " + "چهار";
                                break;
                            case '5':
                                res += " و " + "پنج";
                                break;
                            case '6':
                                res += " و " + "شش";
                                break;
                            case '7':
                                res += " و " + "هفت";
                                break;
                            case '8':
                                res += " و " + "هشت";
                                break;
                            case '9':
                                res += " و " + "نه";
                                break;
                        }
                    }
                    else
                        switch (digit)
                        {
                            case 10:
                                res = res + " و " + "ده";
                                break;
                            case 11:
                                res = res + " و " + "يازده";
                                break;
                            case 12:
                                res = res + " و " + "دوازده";
                                break;
                            case 13:
                                res = res + " و " + "سيزده";
                                break;
                            case 14:
                                res = res + " و " + "چهارده";
                                break;
                            case 15:
                                res = res + " و " + "پانزده";
                                break;
                            case 16:
                                res = res + " و " + "شانزده";
                                break;
                            case 17:
                                res = res + " و " + "هفده";
                                break;
                            case 18:
                                res = res + " و " + "هجده";
                                break;
                            case 19:
                                res = res + " و " + "نونزده";
                                break;
                            case 20:
                                res = res + " و " + "بيست";
                                break;
                        }
                }

                else if (digit > 20)
                {
                    temp = Convert.ToChar(str.Substring(0, 1));
                    switch (temp)
                    {
                        case '2':
                            res = res + " و " + "بيست";
                            break;
                        case '3':
                            res = res + " و " + "سي";
                            break;
                        case '4':
                            res = res + " و " + "چهل";
                            break;
                        case '5':
                            res = res + " و " + "پنجاه";
                            break;
                        case '6':
                            res = res + " و " + "شصت";
                            break;
                        case '7':
                            res = res + " و " + "هفتاد";
                            break;
                        case '8':
                            res = res + " و " + "هشتاد";
                            break;
                        case '9':
                            res = res + " و " + "نود";
                            break;
                    }
                    temp = Convert.ToChar(str.Substring(1, 1));
                    switch (temp)
                    {
                        case '0':
                            break;
                        case '1':
                            res += " و " + "يك";
                            break;
                        case '2':
                            res += " و " + "دو";
                            break;
                        case '3':
                            res += " و " + "سه";
                            break;
                        case '4':
                            res += " و " + "چهار";
                            break;
                        case '5':
                            res += " و " + "پنج";
                            break;
                        case '6':
                            res += " و " + "شش";
                            break;
                        case '7':
                            res += " و " + "هفت";
                            break;
                        case '8':
                            res += " و " + "هشت";
                            break;
                        case '9':
                            res += " و " + "نه";
                            break;
                    }
                }







            }

            else if (mod == 1)
            {
                switch (str)
                {

                    case "0":
                        res = "صفر";
                        break;
                    case "1":
                        res = "يك";
                        break;
                    case "2":
                        res = "دو";
                        break;
                    case "3":
                        res = "سه";
                        break;
                    case "4":
                        res = "چهار";
                        break;
                    case "5":
                        res = "پنج";
                        break;
                    case "6":
                        res = "شش";
                        break;
                    case "7":
                        res = "هفت";
                        break;
                    case "8":
                        res = "هشت";
                        break;
                    case "9":
                        res = "نه";
                        break;
                }
            }

            else if (mod == 2)
            {
                if (digit <= 20)
                    switch (digit)
                    {
                        case 10:
                            res = "ده";
                            break;
                        case 11:
                            res = "يازده";
                            break;
                        case 12:
                            res = "دوازده";
                            break;
                        case 13:
                            res = "سيزده";
                            break;
                        case 14:
                            res = "چهارده";
                            break;
                        case 15:
                            res = "پانزده";
                            break;
                        case 16:
                            res = "شانزده";
                            break;
                        case 17:
                            res = "هفده";
                            break;
                        case 18:
                            res = "هجده";
                            break;
                        case 19:
                            res = "نونزده";
                            break;
                        case 20:
                            res = "بيست";
                            break;
                    }
                else
                {
                    temp = Convert.ToChar(str.Substring(0, 1));
                    switch (temp)
                    {
                        case '2':
                            res = "بيست";
                            break;
                        case '3':
                            res = "سي";
                            break;
                        case '4':
                            res = "چهل";
                            break;
                        case '5':
                            res = "پنجاه";
                            break;
                        case '6':
                            res = "شصت";
                            break;
                        case '7':
                            res = "هفتاد";
                            break;
                        case '8':
                            res = "هشتاد";
                            break;
                        case '9':
                            res = "نود";
                            break;
                    }
                    temp = Convert.ToChar(str.Substring(1, 1));
                    switch (temp)
                    {
                        case '0':
                            break;
                        case '1':
                            res += " و " + "يك";
                            break;
                        case '2':
                            res += " و " + "دو";
                            break;
                        case '3':
                            res += " و " + "سه";
                            break;
                        case '4':
                            res += " و " + "چهار";
                            break;
                        case '5':
                            res += " و " + "پنج";
                            break;
                        case '6':
                            res += " و " + "شش";
                            break;
                        case '7':
                            res += " و " + "هفت";
                            break;
                        case '8':
                            res += " و " + "هشت";
                            break;
                        case '9':
                            res += " و " + "نه";
                            break;
                    }
                }
            }

            return res;
        }

    }
}
