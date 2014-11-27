using System;
using System.Text;

namespace LuceneNetCzechSupport.Stemmers.Snowball
{
    public class CzechStemmerAgressive
    {
        /**
         * A buffer of the current word being stemmed
         */
        private StringBuilder sb = new StringBuilder();
        
        public string Stem(string input)
        {
            //
            input = input.ToLower();
            //reset string buffer
            sb.Remove(0, sb.Length);
            sb.Insert(0, input);
            // stemming...
            //removes case endings from nouns and adjectives
            RemoveCase(sb);
            //removes possesive endings from names -ov- and -in-
            RemovePossessives(sb);
            //removes comparative endings
            RemoveComparative(sb);
            //removes diminutive endings
            RemoveDiminutive(sb);
            //removes augmentatives endings
            RemoveAugmentative(sb);
            //removes derivational sufixes from nouns
            RemoveDerivational(sb);

            var result = sb.ToString();
            return result;
        }
        private void RemoveDerivational(StringBuilder buffer)
        {
            int len = buffer.Length;
            // 
            if ((len > 8) &&
                buffer.ToString(len - 6, len).Equals("obinec"))
            {

                buffer.Remove(len - 6, len);
                return;
            }//len >8
            if (len > 7)
            {
                if (buffer.ToString(len - 5, len).Equals("ion\u00e1\u0159"))
                { // -ionář 

                    buffer.Remove(len - 4, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 5, len).Equals("ovisk") ||
                        buffer.ToString(len - 5, len).Equals("ovstv") ||
                        buffer.ToString(len - 5, len).Equals("ovi\u0161t") ||  //-ovišt
                        buffer.ToString(len - 5, len).Equals("ovn\u00edk"))
                { //-ovník

                    buffer.Remove(len - 5, len);
                    return;
                }
            }//len>7
            if (len > 6)
            {
                if (buffer.ToString(len - 4, len).Equals("\u00e1sek") || // -ásek 
                    buffer.ToString(len - 4, len).Equals("loun") ||
                    buffer.ToString(len - 4, len).Equals("nost") ||
                    buffer.ToString(len - 4, len).Equals("teln") ||
                    buffer.ToString(len - 4, len).Equals("ovec") ||
                    buffer.ToString(len - 5, len).Equals("ov\u00edk") || //-ovík
                    buffer.ToString(len - 4, len).Equals("ovtv") ||
                    buffer.ToString(len - 4, len).Equals("ovin") ||
                    buffer.ToString(len - 4, len).Equals("\u0161tin"))
                { //-štin

                    buffer.Remove(len - 4, len);
                    return;
                }
                if (buffer.ToString(len - 4, len).Equals("enic") ||
                        buffer.ToString(len - 4, len).Equals("inec") ||
                        buffer.ToString(len - 4, len).Equals("itel"))
                {

                    buffer.Remove(len - 3, len);
                    Palatalise(buffer);
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (buffer.ToString(len - 3, len).Equals("\u00e1rn"))
                { //-árn

                    buffer.Remove(len - 3, len);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("\u011bnk"))
                { //-ěnk

                    buffer.Remove(len - 2, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("i\u00e1n") || //-ián
                        buffer.ToString(len - 3, len).Equals("ist") ||
                        buffer.ToString(len - 3, len).Equals("isk") ||
                        buffer.ToString(len - 3, len).Equals("i\u0161t") || //-išt
                        buffer.ToString(len - 3, len).Equals("itb") ||
                        buffer.ToString(len - 3, len).Equals("\u00edrn"))
                {  //-írn

                    buffer.Remove(len - 2, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("och") ||
                        buffer.ToString(len - 3, len).Equals("ost") ||
                        buffer.ToString(len - 3, len).Equals("ovn") ||
                        buffer.ToString(len - 3, len).Equals("oun") ||
                        buffer.ToString(len - 3, len).Equals("out") ||
                        buffer.ToString(len - 3, len).Equals("ou\u0161"))
                {  //-ouš

                    buffer.Remove(len - 3, len);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("u\u0161k"))
                { //-ušk

                    buffer.Remove(len - 3, len);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("kyn") ||
                        buffer.ToString(len - 3, len).Equals("\u010dan") ||    //-čan
                        buffer.ToString(len - 3, len).Equals("k\u00e1\u0159") || //kář
                        buffer.ToString(len - 3, len).Equals("n\u00e9\u0159") || //néř
                        buffer.ToString(len - 3, len).Equals("n\u00edk") ||      //-ník
                        buffer.ToString(len - 3, len).Equals("ctv") ||
                        buffer.ToString(len - 3, len).Equals("stv"))
                {

                    buffer.Remove(len - 3, len);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (buffer.ToString(len - 2, len).Equals("\u00e1\u010d") || // -áč
                    buffer.ToString(len - 2, len).Equals("a\u010d") ||      //-ač
                    buffer.ToString(len - 2, len).Equals("\u00e1n") ||      //-án
                        buffer.ToString(len - 2, len).Equals("an") ||
                        buffer.ToString(len - 2, len).Equals("\u00e1\u0159") || //-ář
                        buffer.ToString(len - 2, len).Equals("as"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("ec") ||
                        buffer.ToString(len - 2, len).Equals("en") ||
                        buffer.ToString(len - 2, len).Equals("\u011bn") ||   //-ěn
                        buffer.ToString(len - 2, len).Equals("\u00e9\u0159"))
                {  //-éř

                    buffer.Remove(len - 1, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("\u00ed\u0159") || //-íř
                        buffer.ToString(len - 2, len).Equals("ic") ||
                        buffer.ToString(len - 2, len).Equals("in") ||
                        buffer.ToString(len - 2, len).Equals("\u00edn") ||  //-ín
                        buffer.ToString(len - 2, len).Equals("it") ||
                        buffer.ToString(len - 2, len).Equals("iv"))
                {

                    buffer.Remove(len - 1, len);
                    Palatalise(buffer);
                    return;
                }

                if (buffer.ToString(len - 2, len).Equals("ob") ||
                        buffer.ToString(len - 2, len).Equals("ot") ||
                        buffer.ToString(len - 2, len).Equals("ov") ||
                        buffer.ToString(len - 2, len).Equals("o\u0148"))
                { //-oň 

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("ul"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("yn"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("\u010dk") ||              //-čk
                        buffer.ToString(len - 2, len).Equals("\u010dn") ||  //-čn
                        buffer.ToString(len - 2, len).Equals("dl") ||
                        buffer.ToString(len - 2, len).Equals("nk") ||
                        buffer.ToString(len - 2, len).Equals("tv") ||
                        buffer.ToString(len - 2, len).Equals("tk") ||
                        buffer.ToString(len - 2, len).Equals("vk"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                if (buffer[buffer.Length - 1] == 'c' ||
                   buffer[buffer.Length - 1] == '\u010d' || //-č
                   buffer[buffer.Length - 1] == 'k' ||
                   buffer[buffer.Length - 1] == 'l' ||
                   buffer[buffer.Length - 1] == 'n' ||
                   buffer[buffer.Length - 1] == 't')
                {

                    buffer.Remove(len - 1, len);
                }
            }//len>3	

        }//removeDerivational

        private void RemoveAugmentative(StringBuilder buffer)
        {
            int len = buffer.Length;
            //
            if ((len > 6) &&
                 buffer.ToString(len - 4, len).Equals("ajzn"))
            {

                buffer.Remove(len - 4, len);
                return;
            }
            if ((len > 5) &&
                (buffer.ToString(len - 3, len).Equals("izn") ||
                 buffer.ToString(len - 3, len).Equals("isk")))
            {

                buffer.Remove(len - 2, len);
                Palatalise(buffer);
                return;
            }
            if ((len > 4) &&
                 buffer.ToString(len - 2, len).Equals("\00e1k"))
            { //-ák

                buffer.Remove(len - 2, len);
            }

        }

        private void RemoveDiminutive(StringBuilder buffer)
        {
            int len = buffer.Length;
            // 
            if ((len > 7) &&
                 buffer.ToString(len - 5, len).Equals("ou\u0161ek"))
            {  //-oušek

                buffer.Remove(len - 5, len);
                return;
            }
            if (len > 6)
            {
                if (buffer.ToString(len - 4, len).Equals("e\u010dek") ||      //-eček
                   buffer.ToString(len - 4, len).Equals("\u00e9\u010dek") ||    //-éček
                   buffer.ToString(len - 4, len).Equals("i\u010dek") ||         //-iček
                   buffer.ToString(len - 4, len).Equals("\u00ed\u010dek") ||    //íček
                   buffer.ToString(len - 4, len).Equals("enek") ||
                   buffer.ToString(len - 4, len).Equals("\u00e9nek") ||      //-ének
                   buffer.ToString(len - 4, len).Equals("inek") ||
                   buffer.ToString(len - 4, len).Equals("\u00ednek"))
                {      //-ínek

                    buffer.Remove(len - 3, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 4, len).Equals("\u00e1\u010dek") || //áček
                     buffer.ToString(len - 4, len).Equals("a\u010dek") ||   //aček
                     buffer.ToString(len - 4, len).Equals("o\u010dek") ||   //oček
                     buffer.ToString(len - 4, len).Equals("u\u010dek") ||   //uček
                     buffer.ToString(len - 4, len).Equals("anek") ||
                     buffer.ToString(len - 4, len).Equals("onek") ||
                     buffer.ToString(len - 4, len).Equals("unek") ||
             buffer.ToString(len - 4, len).Equals("\u00e1nek"))
                {   //-ánek

                    buffer.Remove(len - 4, len);
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (buffer.ToString(len - 3, len).Equals("e\u010dk") ||   //-ečk
                   buffer.ToString(len - 3, len).Equals("\u00e9\u010dk") ||  //-éčk 
                   buffer.ToString(len - 3, len).Equals("i\u010dk") ||   //-ičk
                   buffer.ToString(len - 3, len).Equals("\u00ed\u010dk") ||    //-íčk
                   buffer.ToString(len - 3, len).Equals("enk") ||   //-enk
                   buffer.ToString(len - 3, len).Equals("\u00e9nk") ||  //-énk 
                   buffer.ToString(len - 3, len).Equals("ink") ||   //-ink
                   buffer.ToString(len - 3, len).Equals("\u00ednk"))
                {   //-ínk

                    buffer.Remove(len - 3, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("\u00e1\u010dk") ||  //-áčk
                    buffer.ToString(len - 3, len).Equals("au010dk") || //-ačk
                    buffer.ToString(len - 3, len).Equals("o\u010dk") ||  //-očk
                    buffer.ToString(len - 3, len).Equals("u\u010dk") ||   //-učk 
                    buffer.ToString(len - 3, len).Equals("ank") ||
                    buffer.ToString(len - 3, len).Equals("onk") ||
                    buffer.ToString(len - 3, len).Equals("unk"))
                {

                    buffer.Remove(len - 3, len);
                    return;

                }
                if (buffer.ToString(len - 3, len).Equals("\u00e1tk") || //-átk
                   buffer.ToString(len - 3, len).Equals("\u00e1nk") ||  //-ánk
           buffer.ToString(len - 3, len).Equals("u\u0161k"))
                {   //-ušk

                    buffer.Remove(len - 3, len);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (buffer.ToString(len - 2, len).Equals("ek") ||
                   buffer.ToString(len - 2, len).Equals("\u00e9k") ||  //-ék
                   buffer.ToString(len - 2, len).Equals("\u00edk") ||  //-ík
                   buffer.ToString(len - 2, len).Equals("ik"))
                {

                    buffer.Remove(len - 1, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("\u00e1k") ||  //-ák
                    buffer.ToString(len - 2, len).Equals("ak") ||
                    buffer.ToString(len - 2, len).Equals("ok") ||
                    buffer.ToString(len - 2, len).Equals("uk"))
                {

                    buffer.Remove(len - 1, len);
                    return;
                }
            }
            if ((len > 3) &&
                 buffer.ToString(len - 1, len).Equals("k"))
            {

                buffer.Remove(len - 1, len);
            }
        }//removeDiminutives

        private void RemoveComparative(StringBuilder buffer)
        {
            int len = buffer.Length;
            // 
            if ((len > 5) &&
                (buffer.ToString(len - 3, len).Equals("ej\u0161") ||  //-ejš
                 buffer.ToString(len - 3, len).Equals("\u011bj\u0161")))
            {   //-ějš

                buffer.Remove(len - 2, len);
                Palatalise(buffer);
            }

        }

        private void Palatalise(StringBuilder buffer)
        {
            int len = buffer.Length;

            if (buffer.ToString(len - 2, len).Equals("ci") ||
                 buffer.ToString(len - 2, len).Equals("ce") ||
                 buffer.ToString(len - 2, len).Equals("\u010di") ||      //-či
                 buffer.ToString(len - 2, len).Equals("\u010de"))
            {   //-če

                buffer.Replace(len - 2, len, "k");
                return;
            }
            if (buffer.ToString(len - 2, len).Equals("zi") ||
                 buffer.ToString(len - 2, len).Equals("ze") ||
                 buffer.ToString(len - 2, len).Equals("\u017ei") ||    //-ži
                 buffer.ToString(len - 2, len).Equals("\u017ee"))
            {  //-že

                buffer.Replace(len - 2, len, "h");
                return;
            }
            if (buffer.ToString(len - 3, len).Equals("\u010dt\u011b") ||     //-čtě
                 buffer.ToString(len - 3, len).Equals("\u010dti") ||   //-čti
                 buffer.ToString(len - 3, len).Equals("\u010dt\u00ed"))
            {   //-čtí

                buffer.Replace(len - 3, len, "ck");
                return;
            }
            if (buffer.ToString(len - 2, len).Equals("\u0161t\u011b") ||   //-ště
                buffer.ToString(len - 2, len).Equals("\u0161ti") ||   //-šti
                 buffer.ToString(len - 2, len).Equals("\u0161t\u00ed"))
            {  //-ští

                buffer.Replace(len - 2, len, "sk");
                return;
            }
            buffer.Remove(len - 1, len);
        }//palatalise

        private void RemovePossessives(StringBuilder buffer)
        {
            int len = buffer.Length;

            if (len > 5)
            {
                if (buffer.ToString(len - 2, len).Equals("ov"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("\u016fv"))
                { //-ův

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("in"))
                {

                    buffer.Remove(len - 1, len);
                    Palatalise(buffer);
                }
            }
        }//removePossessives

        private void RemoveCase(StringBuilder buffer)
        {
            int len = buffer.Length;
            // 
            if ((len > 7) &&
                 buffer.ToString(len - 5, len).Equals("atech"))
            {

                buffer.Remove(len - 5, len);
                return;
            }//len>7
            if (len > 6)
            {
                if (buffer.ToString(len - 4, len).Equals("\u011btem"))
                {   //-ětem

                    buffer.Remove(len - 3, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 4, len).Equals("at\u016fm"))
                {  //-atům
                    buffer.Remove(len - 4, len);
                    return;
                }

            }
            if (len > 5)
            {
                if (buffer.ToString(len - 3, len).Equals("ech") ||
                      buffer.ToString(len - 3, len).Equals("ich") ||
              buffer.ToString(len - 3, len).Equals("\u00edch"))
                { //-ích

                    buffer.Remove(len - 2, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("\u00e9ho") || //-ého
    buffer.ToString(len - 3, len).Equals("\u011bmi") ||  //-ěmu
    buffer.ToString(len - 3, len).Equals("emi") ||
    buffer.ToString(len - 3, len).Equals("\u00e9mu") ||  // -ému				                                                                buffer.ToString( len-3,len).Equals("ete")||
    buffer.ToString(len - 3, len).Equals("eti") ||
    buffer.ToString(len - 3, len).Equals("iho") ||
    buffer.ToString(len - 3, len).Equals("\u00edho") ||  //-ího
    buffer.ToString(len - 3, len).Equals("\u00edmi") ||  //-ími
    buffer.ToString(len - 3, len).Equals("imu"))
                {

                    buffer.Remove(len - 2, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 3, len).Equals("\u00e1ch") || //-ách
     buffer.ToString(len - 3, len).Equals("ata") ||
     buffer.ToString(len - 3, len).Equals("aty") ||
     buffer.ToString(len - 3, len).Equals("\u00fdch") ||   //-ých
     buffer.ToString(len - 3, len).Equals("ama") ||
     buffer.ToString(len - 3, len).Equals("ami") ||
     buffer.ToString(len - 3, len).Equals("ov\u00e9") ||   //-ové
     buffer.ToString(len - 3, len).Equals("ovi") ||
     buffer.ToString(len - 3, len).Equals("\u00fdmi"))
                {  //-ými

                    buffer.Remove(len - 3, len);
                    return;
                }
            }
            if (len > 4)
            {
                if (buffer.ToString(len - 2, len).Equals("em"))
                {

                    buffer.Remove(len - 1, len);
                    Palatalise(buffer);
                    return;

                }
                if (buffer.ToString(len - 2, len).Equals("es") ||
             buffer.ToString(len - 2, len).Equals("\u00e9m") ||    //-ém
             buffer.ToString(len - 2, len).Equals("\u00edm"))
                {   //-ím

                    buffer.Remove(len - 2, len);
                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("\u016fm"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
                if (buffer.ToString(len - 2, len).Equals("at") ||
             buffer.ToString(len - 2, len).Equals("\u00e1m") ||    //-ám
             buffer.ToString(len - 2, len).Equals("os") ||
             buffer.ToString(len - 2, len).Equals("us") ||
             buffer.ToString(len - 2, len).Equals("\u00fdm") ||     //-ým
             buffer.ToString(len - 2, len).Equals("mi") ||
             buffer.ToString(len - 2, len).Equals("ou"))
                {

                    buffer.Remove(len - 2, len);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                if (buffer.ToString(len - 1, len).Equals("e") ||
                   buffer.ToString(len - 1, len).Equals("i"))
                {

                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 1, len).Equals("\u00ed") ||    //-é
            buffer.ToString(len - 1, len).Equals("\u011b"))
                {   //-ě

                    Palatalise(buffer);
                    return;
                }
                if (buffer.ToString(len - 1, len).Equals("u") ||
             buffer.ToString(len - 1, len).Equals("y") ||
             buffer.ToString(len - 1, len).Equals("\u016f"))
                {   //-ů

                    buffer.Remove(len - 1, len);
                    return;
                }
                if (buffer.ToString(len - 1, len).Equals("a") ||
                    buffer.ToString(len - 1, len).Equals("o") ||
            buffer.ToString(len - 1, len).Equals("\u00e1") ||  // -á
            buffer.ToString(len - 1, len).Equals("\u00e9") ||  //-é
            buffer.ToString(len - 1, len).Equals("\u00fd"))
                {   //-ý

                    buffer.Remove(len - 1, len);
                }
            }//len>3
        }
    }

    internal static class StringBuilderExtensions
    {
        public static StringBuilder Replace(this StringBuilder sb, int startIndex, int length, string replaceText)
        {
            sb.Remove(startIndex, length);
            sb.Insert(startIndex, replaceText);

            return sb;
        }
    }
}
