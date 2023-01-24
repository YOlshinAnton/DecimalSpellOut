using System;
using System.Text;

namespace DecimalSpellOut.DecimalExtension
{
    public static class DecimalExtension
    {
        public static string SpellOutRU(this decimal d)
        {
            if (d > 999_999_999_999.99m)
            {
                throw new ArgumentException("the sum is too large");
            }

            var rub = Convert.ToInt64(decimal.Floor(d));
            var kop = Convert.ToInt64(decimal.Round((d - rub) * 100));

            var sb = new StringBuilder();

            // spell out rub
            if (rub >= 0) 
            {
                sb.Append((rub % 20) switch {
                    1 => "рубль",
                    2 => "рубля",
                    3 => "рубля",
                    4 => "рубля",
                    _ => "рублей"
                });
            }
            if (rub == 0)
            {
                sb.Insert(0, "ноль");
            }
            for (int exp = 0; rub > 0; ++exp)
            {
                long number = rub % 1000;
                rub /= 1000;
                sb.Insert(0, _SpellOutExponentRU(exp, number));
                sb.Insert(0, ' ');
                sb.Insert(0, _SpellOutNumberRU(exp, number));
            }

            // spell out kop
            if (kop > 0) 
            {
                sb.Append(' ');
                sb.Append(kop.ToString("D2"));
                sb.Append(' ');
                sb.Append((kop % 20) switch {
                    1 => "копейка",
                    2 => "копейки",
                    3 => "копейки",
                    4 => "копейки",
                    _ => "копеек"
                });
            }
            
            return sb.ToString();
        }

        private static string _SpellOutExponentRU(int exponentNumber, long number)
        {
            if (exponentNumber == 0)
            {
                return string.Empty;
            }

            List<string[]> exponents = new List<string[]>
            {
                new string[] { "тысячь ", "тысяча ", "тысячи " },
                new string[] { "милионов ", "милион ", "милиона " },
                new string[] { "милиардов ", "милиард ", "милиарда " }
            };
            string[] exponent = exponents[exponentNumber - 1];

            return (number % 20) switch
            {
                1 => exponent[1],
                2 => exponent[2],
                3 => exponent[2],
                4 => exponent[2],
                _ => exponent[0]
            };
        }

        private static string _SpellOutNumberRU(int exponentNumber, long number)
        {
            if (number == 0) return string.Empty;

            long absNumber = Math.Abs(number);

            StringBuilder sb = new StringBuilder();

            sb.Append((absNumber / 100) switch
            {
                1 => "сто ",
                2 => "двести ",
                3 => "триста ",
                4 => "четыреста ",
                5 => "пятьсот ",
                6 => "шестьсот ",
                7 => "семьсот ",
                8 => "восемьсот ",
                9 => "девятьсот ",
                _ => ""
            });

            sb.Append((absNumber / 10 % 10) switch
            {
                2 => "двадцать ",
                3 => "тридцать ",
                4 => "сорок ",
                5 => "пятьдесят ",
                6 => "шестьдесят ",
                7 => "семьдесят ",
                8 => "восемьдесят ",
                9 => "девяносто ",
                _ => ""
            });

            string[] one = { "одна ", "один " };
            string[] two = { "две ", "два " };

            sb.Append((absNumber >= 20 ? absNumber % 10 : absNumber % 20) switch
            {
                1 => one[exponentNumber == 1 ? 0 : 1],
                2 => two[exponentNumber == 1 ? 0 : 1],
                3 => "три ",
                4 => "четыре ",
                5 => "пять ",
                6 => "шесть ",
                7 => "семь ",
                8 => "восемь ",
                9 => "девять ",
                10 => "десять ",
                11 => "одинадцать ",
                12 => "двенадцать ",
                13 => "тринадцать ",
                14 => "четырнадцать ",
                15 => "пятнадцать ",
                16 => "шестнадцать ",
                17 => "семьнадцать ",
                18 => "восемнадцать ",
                19 => "девятнадцать ",
                _ => ""
            });
            return sb.ToString().Trim();
        }   
    }
}

