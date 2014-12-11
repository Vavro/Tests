// This file was generated automatically by the Snowball to C# compiler

using System;
using System.Text;
using SF.Snowball;

namespace Snowball.Stemmer
{

	/**
	 * Generated class implementing code defined by a snowball script.
	 */
	public class CzechAggresiveStemmer : SnowballProgram
	{

		public CzechAggresiveStemmer()
		{
			a_0 = new Among[] {
				new Among("ce", -1, 1, "", this),
				new Among("ze", -1, 2, "", this),
				new Among("\u00BEe", -1, 2, "", this),
				new Among("ci", -1, 1, "", this),
				new Among("\u00B9ti", -1, 4, "", this),
				new Among("\u00E8ti", -1, 3, "", this),
				new Among("zi", -1, 2, "", this),
				new Among("\u00BEi", -1, 2, "", this),
				new Among("\u00E8i", -1, 1, "", this),
				new Among("\u00E8", -1, 1, "", this),
				new Among("\u00B9t\u00E9", -1, 4, "", this),
				new Among("\u00E8t\u00E9", -1, 3, "", this),
				new Among("\u00B9t\u00EC", -1, 4, "", this),
				new Among("\u00E8t\u00EC", -1, 3, "", this)
			};

			a_1 = new Among[] {
				new Among("in", -1, 2, "", this),
				new Among("ov", -1, 1, "", this),
				new Among("\u00F9v", -1, 1, "", this)
			};

			a_2 = new Among[] {
				new Among("a", -1, 1, "", this),
				new Among("ama", 0, 1, "", this),
				new Among("ata", 0, 1, "", this),
				new Among("e", -1, 2, "", this),
				new Among("\u00ECte", 3, 2, "", this),
				new Among("ech", -1, 2, "", this),
				new Among("atech", 5, 1, "", this),
				new Among("ich", -1, 2, "", this),
				new Among("\u00E1ch", -1, 1, "", this),
				new Among("\u00EDch", -1, 2, "", this),
				new Among("\u00FDch", -1, 1, "", this),
				new Among("i", -1, 2, "", this),
				new Among("mi", 11, 1, "", this),
				new Among("ami", 12, 1, "", this),
				new Among("emi", 12, 2, "", this),
				new Among("\u00ECmi", 12, 2, "", this),
				new Among("\u00EDmi", 12, 2, "", this),
				new Among("\u00FDmi", 12, 1, "", this),
				new Among("\u00ECti", 11, 2, "", this),
				new Among("ovi", 11, 1, "", this),
				new Among("em", -1, 3, "", this),
				new Among("\u00ECtem", 20, 1, "", this),
				new Among("\u00E1m", -1, 1, "", this),
				new Among("\u00E9m", -1, 2, "", this),
				new Among("\u00EDm", -1, 2, "", this),
				new Among("at\u00F9m", -1, 1, "", this),
				new Among("\u00FDm", -1, 1, "", this),
				new Among("o", -1, 1, "", this),
				new Among("iho", 27, 2, "", this),
				new Among("\u00E9ho", 27, 2, "", this),
				new Among("\u00EDho", 27, 2, "", this),
				new Among("es", -1, 2, "", this),
				new Among("os", -1, 1, "", this),
				new Among("us", -1, 1, "", this),
				new Among("at", -1, 1, "", this),
				new Among("u", -1, 1, "", this),
				new Among("imu", 35, 2, "", this),
				new Among("\u00E9mu", 35, 2, "", this),
				new Among("ou", 35, 1, "", this),
				new Among("y", -1, 1, "", this),
				new Among("aty", 39, 1, "", this),
				new Among("\u00E1", -1, 1, "", this),
				new Among("\u00E9", -1, 1, "", this),
				new Among("ov\u00E9", 42, 1, "", this),
				new Among("\u00EC", -1, 2, "", this),
				new Among("\u00ED", -1, 2, "", this),
				new Among("\u00F9", -1, 1, "", this),
				new Among("\u00FD", -1, 1, "", this)
			};

			a_3 = new Among[] {
				new Among("ob", -1, 1, "", this),
				new Among("itb", -1, 2, "", this),
				new Among("ec", -1, 3, "", this),
				new Among("inec", 2, 2, "", this),
				new Among("obinec", 3, 1, "", this),
				new Among("ovec", 2, 1, "", this),
				new Among("ic", -1, 2, "", this),
				new Among("enic", 6, 3, "", this),
				new Among("och", -1, 1, "", this),
				new Among("\u00E1sek", -1, 1, "", this),
				new Among("nk", -1, 1, "", this),
				new Among("isk", -1, 2, "", this),
				new Among("ovisk", 11, 1, "", this),
				new Among("tk", -1, 1, "", this),
				new Among("vk", -1, 1, "", this),
				new Among("i\u00B9k", -1, 2, "", this),
				new Among("u\u00B9k", -1, 1, "", this),
				new Among("\u00E8k", -1, 1, "", this),
				new Among("n\u00EDk", -1, 1, "", this),
				new Among("ovn\u00EDk", 18, 1, "", this),
				new Among("ov\u00EDk", -1, 1, "", this),
				new Among("dl", -1, 1, "", this),
				new Among("itel", -1, 2, "", this),
				new Among("ul", -1, 1, "", this),
				new Among("an", -1, 1, "", this),
				new Among("\u00E8an", 24, 1, "", this),
				new Among("en", -1, 3, "", this),
				new Among("in", -1, 2, "", this),
				new Among("\u00B9tin", 27, 1, "", this),
				new Among("ovin", 27, 1, "", this),
				new Among("teln", -1, 1, "", this),
				new Among("\u00E1rn", -1, 1, "", this),
				new Among("\u00EDrn", -1, 6, "", this),
				new Among("oun", -1, 1, "", this),
				new Among("loun", 33, 1, "", this),
				new Among("ovn", -1, 1, "", this),
				new Among("yn", -1, 1, "", this),
				new Among("kyn", 36, 1, "", this),
				new Among("\u00E1n", -1, 1, "", this),
				new Among("i\u00E1n", 38, 2, "", this),
				new Among("\u00E8n", -1, 1, "", this),
				new Among("\u00ECn", -1, 5, "", this),
				new Among("\u00EDn", -1, 6, "", this),
				new Among("as", -1, 1, "", this),
				new Among("it", -1, 2, "", this),
				new Among("ot", -1, 1, "", this),
				new Among("ist", -1, 2, "", this),
				new Among("ost", -1, 1, "", this),
				new Among("nost", 47, 1, "", this),
				new Among("out", -1, 1, "", this),
				new Among("ovi\u00B9t", -1, 1, "", this),
				new Among("iv", -1, 2, "", this),
				new Among("ov", -1, 1, "", this),
				new Among("tv", -1, 1, "", this),
				new Among("ctv", 53, 1, "", this),
				new Among("stv", 53, 1, "", this),
				new Among("ovstv", 55, 1, "", this),
				new Among("ovtv", 53, 1, "", this),
				new Among("ou\u00B9", -1, 1, "", this),
				new Among("a\u00E8", -1, 1, "", this),
				new Among("\u00E1\u00E8", -1, 1, "", this),
				new Among("o\u00F2", -1, 1, "", this),
				new Among("\u00E1\u00F8", -1, 1, "", this),
				new Among("k\u00E1\u00F8", 62, 1, "", this),
				new Among("ion\u00E1\u00F8", 62, 2, "", this),
				new Among("\u00E9\u00F8", -1, 4, "", this),
				new Among("n\u00E9\u00F8", 65, 1, "", this),
				new Among("\u00ED\u00F8", -1, 6, "", this)
			};

			a_4 = new Among[] {
				new Among("c", -1, 1, "", this),
				new Among("k", -1, 1, "", this),
				new Among("l", -1, 1, "", this),
				new Among("n", -1, 1, "", this),
				new Among("t", -1, 1, "", this),
				new Among("\u00E8", -1, 1, "", this)
			};

			a_5 = new Among[] {
				new Among("isk", -1, 2, "", this),
				new Among("\u00E1k", -1, 1, "", this),
				new Among("izn", -1, 2, "", this),
				new Among("ajzn", -1, 1, "", this)
			};

			a_6 = new Among[] {
				new Among("k", -1, 1, "", this),
				new Among("ak", 0, 7, "", this),
				new Among("ek", 0, 2, "", this),
				new Among("anek", 2, 1, "", this),
				new Among("enek", 2, 2, "", this),
				new Among("inek", 2, 4, "", this),
				new Among("onek", 2, 1, "", this),
				new Among("unek", 2, 1, "", this),
				new Among("\u00E1nek", 2, 1, "", this),
				new Among("ou\u00B9ek", 2, 1, "", this),
				new Among("a\u00E8ek", 2, 1, "", this),
				new Among("e\u00E8ek", 2, 2, "", this),
				new Among("i\u00E8ek", 2, 4, "", this),
				new Among("o\u00E8ek", 2, 1, "", this),
				new Among("u\u00E8ek", 2, 1, "", this),
				new Among("\u00E1\u00E8ek", 2, 1, "", this),
				new Among("\u00E9\u00E8ek", 2, 3, "", this),
				new Among("\u00ED\u00E8ek", 2, 5, "", this),
				new Among("ik", 0, 4, "", this),
				new Among("ank", 0, 1, "", this),
				new Among("enk", 0, 1, "", this),
				new Among("ink", 0, 1, "", this),
				new Among("onk", 0, 1, "", this),
				new Among("unk", 0, 1, "", this),
				new Among("\u00E1nk", 0, 1, "", this),
				new Among("\u00E9nk", 0, 1, "", this),
				new Among("\u00EDnk", 0, 1, "", this),
				new Among("ok", 0, 8, "", this),
				new Among("\u00E1tk", 0, 1, "", this),
				new Among("uk", 0, 9, "", this),
				new Among("u\u00B9k", 0, 1, "", this),
				new Among("\u00E1k", 0, 6, "", this),
				new Among("a\u00E8k", 0, 1, "", this),
				new Among("e\u00E8k", 0, 1, "", this),
				new Among("i\u00E8k", 0, 1, "", this),
				new Among("o\u00E8k", 0, 1, "", this),
				new Among("u\u00E8k", 0, 1, "", this),
				new Among("\u00E1\u00E8k", 0, 1, "", this),
				new Among("\u00E9\u00E8k", 0, 1, "", this),
				new Among("\u00ED\u00E8k", 0, 1, "", this),
				new Among("\u00E9k", 0, 3, "", this),
				new Among("\u00EDk", 0, 5, "", this)
			};

			a_7 = new Among[] {
				new Among("ej\u00B9", -1, 2, "", this),
				new Among("\u00ECj\u00B9", -1, 1, "", this)
			};

		}

		private Among[] a_0;
		private Among[] a_1;
		private Among[] a_2;
		private Among[] a_3;
		private Among[] a_4;
		private Among[] a_5;
		private Among[] a_6;
		private Among[] a_7;
		private static readonly char[] g_v = new char[] {(char)17, (char)65, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)1, (char)25, (char)4, (char)19 };

        private int I_p1;
        private int I_pV;

		private void copy_from(CzechAggresiveStemmer other) {
			I_p1 = other.I_p1;
			I_pV = other.I_pV;
			base.copy_from(other);
		}

		private bool r_mark_regions() {
			int v_1;
			// (, line 43
			I_pV = limit;
			I_p1 = limit;
			// do, line 48
			v_1 = cursor;
			// (, line 48
			// gopast, line 49
			while(true)
			{
				if (!(out_grouping(g_v, 97, 253)))
				{
					goto lab2;
				}
				goto golab1;
			lab2:
				if (cursor >= limit)
				{
					goto lab0;
				}
				cursor++;
			}
		golab1:
			// setmark pV, line 49
			I_pV = cursor;
			// gopast, line 50
			while(true)
			{
				if (!(out_grouping(g_v, 97, 253)))
				{
					goto lab4;
				}
				goto golab3;
			lab4:
				if (cursor >= limit)
				{
					goto lab0;
				}
				cursor++;
			}
		golab3:
			// gopast, line 50
			while(true)
			{
				if (!(in_grouping(g_v, 97, 253)))
				{
					goto lab6;
				}
				goto golab5;
			lab6:
				if (cursor >= limit)
				{
					goto lab0;
				}
				cursor++;
			}
		golab5:
			// setmark p1, line 50
			I_p1 = cursor;
		lab0:
			cursor = v_1;
			return true;
		}

		private bool r_RV() {
			if (!(I_pV <= cursor))
			{
				return false;
			}
			return true;
		}

		private bool r_R1() {
			if (!(I_p1 <= cursor))
			{
				return false;
			}
			return true;
		}

		private bool r_palatalise() {
			int among_var;
			// (, line 59
			// [, line 60
			ket = cursor;
			// substring, line 60
			among_var = find_among_b(a_0, 14);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 60
			bra = cursor;
			// call RV, line 60
			if (!r_RV())
			{
				return false;
			}
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 62
					// <-, line 62
					slice_from("k");
					break;
				case 2:
					// (, line 64
					// <-, line 64
					slice_from("h");
					break;
				case 3:
					// (, line 66
					// <-, line 66
					slice_from("ck");
					break;
				case 4:
					// (, line 68
					// <-, line 68
					slice_from("sk");
					break;
			}
			return true;
		}

		private bool r_do_possessive() {
			int among_var;
			int v_1;
			// (, line 72
			// [, line 73
			ket = cursor;
			// substring, line 73
			among_var = find_among_b(a_1, 3);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 73
			bra = cursor;
			// call RV, line 73
			if (!r_RV())
			{
				return false;
			}
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 75
					// delete, line 75
					slice_del();
					break;
				case 2:
					// (, line 77
					// delete, line 78
					slice_del();
					// try, line 79
					v_1 = limit - cursor;
					// call palatalise, line 79
					if (!r_palatalise())
					{
						cursor = limit - v_1;
						goto lab0;
					}
				lab0:
					break;
			}
			return true;
		}

		private bool r_do_case() {
			int among_var;
			int v_1;
			int v_2;
			// (, line 84
			// [, line 85
			ket = cursor;
			// substring, line 85
			among_var = find_among_b(a_2, 48);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 85
			bra = cursor;
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 92
					// delete, line 92
					slice_del();
					break;
				case 2:
					// (, line 98
					// delete, line 99
					slice_del();
					// try, line 100
					v_1 = limit - cursor;
					// call palatalise, line 100
					if (!r_palatalise())
					{
						cursor = limit - v_1;
						goto lab0;
					}
				lab0:
					break;
				case 3:
					// (, line 103
					// <-, line 104
					slice_from("e");
					// try, line 105
					v_2 = limit - cursor;
					// call palatalise, line 105
					if (!r_palatalise())
					{
						cursor = limit - v_2;
						goto lab1;
					}
				lab1:
					break;
			}
			return true;
		}

		private bool r_do_derivational() {
			int among_var;
			// (, line 110
			// [, line 111
			ket = cursor;
			// substring, line 111
			among_var = find_among_b(a_3, 68);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 111
			bra = cursor;
			// call R1, line 111
			if (!r_R1())
			{
				return false;
			}
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 120
					// delete, line 120
					slice_del();
					break;
				case 2:
					// (, line 125
					// <-, line 126
					slice_from("i");
					// call palatalise, line 127
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 3:
					// (, line 130
					// <-, line 131
					slice_from("e");
					// call palatalise, line 132
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 4:
					// (, line 135
					// <-, line 136
					slice_from("\u00E9");
					// call palatalise, line 137
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 5:
					// (, line 140
					// <-, line 141
					slice_from("\u00EC");
					// call palatalise, line 142
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 6:
					// (, line 146
					// <-, line 147
					slice_from("\u00ED");
					// call palatalise, line 148
					if (!r_palatalise())
					{
						return false;
					}
					break;
			}
			return true;
		}

		private bool r_do_deriv_single() {
			int among_var;
			// (, line 152
			// [, line 153
			ket = cursor;
			// substring, line 153
			among_var = find_among_b(a_4, 6);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 153
			bra = cursor;
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 155
					// delete, line 155
					slice_del();
					break;
			}
			return true;
		}

		private bool r_do_augmentative() {
			int among_var;
			// (, line 160
			// [, line 161
			ket = cursor;
			// substring, line 161
			among_var = find_among_b(a_5, 4);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 161
			bra = cursor;
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 163
					// delete, line 163
					slice_del();
					break;
				case 2:
					// (, line 165
					// <-, line 166
					slice_from("i");
					// call palatalise, line 167
					if (!r_palatalise())
					{
						return false;
					}
					break;
			}
			return true;
		}

		private bool r_do_diminutive() {
			int among_var;
			// (, line 172
			// [, line 173
			ket = cursor;
			// substring, line 173
			among_var = find_among_b(a_6, 42);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 173
			bra = cursor;
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 180
					// delete, line 180
					slice_del();
					break;
				case 2:
					// (, line 182
					// <-, line 183
					slice_from("e");
					// call palatalise, line 184
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 3:
					// (, line 187
					// <-, line 188
					slice_from("\u00E9");
					// call palatalise, line 189
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 4:
					// (, line 192
					// <-, line 193
					slice_from("i");
					// call palatalise, line 194
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 5:
					// (, line 197
					// <-, line 198
					slice_from("\u00ED");
					// call palatalise, line 199
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 6:
					// (, line 202
					// <-, line 202
					slice_from("\u00E1");
					break;
				case 7:
					// (, line 204
					// <-, line 204
					slice_from("a");
					break;
				case 8:
					// (, line 206
					// <-, line 206
					slice_from("o");
					break;
				case 9:
					// (, line 208
					// <-, line 208
					slice_from("u");
					break;
			}
			return true;
		}

		private bool r_do_comparative() {
			int among_var;
			// (, line 212
			// [, line 213
			ket = cursor;
			// substring, line 213
			among_var = find_among_b(a_7, 2);
			if (among_var == 0)
			{
				return false;
			}
			// ], line 213
			bra = cursor;
			switch(among_var) {
				case 0:
					return false;
				case 1:
					// (, line 215
					// <-, line 216
					slice_from("\u00EC");
					// call palatalise, line 217
					if (!r_palatalise())
					{
						return false;
					}
					break;
				case 2:
					// (, line 220
					// <-, line 221
					slice_from("e");
					// call palatalise, line 222
					if (!r_palatalise())
					{
						return false;
					}
					break;
			}
			return true;
		}

		private bool r_do_aggressive() {
			int v_1;
			int v_2;
			int v_3;
			int v_4;
			// (, line 227
			// do, line 228
			v_1 = limit - cursor;
			// call do_comparative, line 228
			if (!r_do_comparative())
			{
				goto lab0;
			}
		lab0:
			cursor = limit - v_1;
			// do, line 229
			v_2 = limit - cursor;
			// call do_diminutive, line 229
			if (!r_do_diminutive())
			{
				goto lab1;
			}
		lab1:
			cursor = limit - v_2;
			// do, line 230
			v_3 = limit - cursor;
			// call do_augmentative, line 230
			if (!r_do_augmentative())
			{
				goto lab2;
			}
		lab2:
			cursor = limit - v_3;
			// or, line 231
			v_4 = limit - cursor;
			// call do_derivational, line 231
			if (!r_do_derivational())
			{
				goto lab4;
			}
			goto lab3;
		lab4:
			cursor = limit - v_4;
			// call do_deriv_single, line 231
			if (!r_do_deriv_single())
			{
				return false;
			}
		lab3:
			return true;
		}

		public override bool Stem() {
			int v_1;
			// (, line 235
			// do, line 236
			v_1 = cursor;
			// call mark_regions, line 236
			if (!r_mark_regions())
			{
				goto lab0;
			}
		lab0:
			cursor = v_1;
			// backwards, line 237
			limit_backward = cursor; cursor = limit;
			// (, line 237
			// call do_case, line 238
			if (!r_do_case())
			{
				return false;
			}
			// call do_possessive, line 239
			if (!r_do_possessive())
			{
				return false;
			}
			// call do_aggressive, line 242
			if (!r_do_aggressive())
			{
				return false;
			}
			cursor = limit_backward;
			return true;
		}
	}
}
