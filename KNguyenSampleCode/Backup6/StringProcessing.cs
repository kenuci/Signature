using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace KNguyenSampleCode
{
	public class StringProcessing
	{
		/// <summary>
		/// Strips out HTML encoding from a text string.
		/// </summary>
		/// <param name="strValue">String value from which to remove HTML encoding.</param>
		/// <param name="blnStrict">Switch to either process all HTML in strict mode.</param>
		/// <returns>HTML-stripped string value.</returns>
		public static string StripHtml( string strValue, bool blnStrict){
			Regex objRegex = null;
			Match objMatch = null;

			string strRuleScriptBlock = "(<script[^>]*>.*?</script>)";
			string strRuleComment = "(<!--.*?-->)";
			string strRuleTag = "([<][^>]*>)";

			string[,] lstRegExPattern = blnStrict?
				new string[,]{
						{strRuleScriptBlock, string.Empty},//strip out script blocks tags
						{strRuleComment, string.Empty},  //strip out html comments
						{strRuleTag, string.Empty} //strip out html tags
				}
				: new string[,]{
						{strRuleScriptBlock, string.Empty},//strip out script blocks tags
						{strRuleComment, string.Empty},  //strip out html comments
						{"([<]br[^>]*>)", "\r\n"}, //convert breaks to newline
						{"([<]/?p[^>]*>)", "\r\n"}, //convert paragraph breaks to newline
						{"([<]p /[^>]*>)", "\r\n"}, //convert paragraph breaks to newline
						{"&nbsp;", " "}, //remove no-space-breaks
						{strRuleTag, string.Empty} //strip out html tags
				};

			for( int intIdx=0; intIdx<lstRegExPattern.GetLength(0); intIdx++){
				objRegex = new Regex( lstRegExPattern[intIdx,0], RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

				while( (objMatch=objRegex.Match(strValue)).Success) //strip out script blocks tags
					strValue = strValue.Replace( objMatch.ToString(), lstRegExPattern[intIdx,1]);
			}

			objRegex = null;
			objMatch = null;

			return strValue;
		}

		/// <summary>
		/// Trims text down at sentence-boundary with specific max character count, max word count rules, whichever yields lesser text.
		/// </summary>
		/// <param name="strText">Input text.</param>
		/// <param name="intMaxCharCount">Max character count to truncate to.</param>
		/// <param name="intMaxWordCount">Max word count to truncate to.</param>
		/// <param name="blnTrueWordBoundary">Switch to determine whether to truncate at word (instead of sentence) boundary.</param>
		/// <returns>Trimmed string.</returns>
		public static string TrimText( string strText, int intMaxCharCount, int intMaxWordCount, bool blnTrueWordBoundary){
			strText = StripHtml( strText, false).Replace( "\r\n", "\n");

			ArrayList lstMarker = new ArrayList();
			lstMarker.Add( ". ");
			lstMarker.Add( "!");
			lstMarker.Add( "?");

			if( blnTrueWordBoundary){ //search for other markers (delimiters that constitute a word boundary)
				lstMarker.Add( " ");
				lstMarker.Add( "\t");
				lstMarker.Add( "\n");
			}

			StringBuilder objStringMarker = new StringBuilder();

			foreach( string strMarker in lstMarker)
				objStringMarker.Append( strMarker.Replace( ". ", "."));

			//first, verify truncation is necessary
			if(
				(
					blnTrueWordBoundary 
					||
					!blnTrueWordBoundary && ( strText.IndexOf(".")!=-1 || strText.IndexOf("!")!=-1 || strText.IndexOf("?")!=-1) //..a sentence exists
				)
				&& ( intMaxCharCount<=0 || ( intMaxCharCount>0 && strText.Length<=intMaxCharCount)) //for max char cap, check if within limit
				&& ( intMaxWordCount<=0 || ( intMaxWordCount>0 && strText.Split( objStringMarker.ToString().ToCharArray()).Length<=intMaxWordCount)) //for max word cap, check if within limit
			)
				return strText;

			if( blnTrueWordBoundary)
				strText = string.Format( "{0} ", strText); //force end marker to ensure inclusion of last word

			//next, start truncation logic because one of the max caps (word count or char count) was exceeded
			int intCurrPos=0;
			int intPrevPos=0;

			int intWordCount=0;

			while( true){
				intCurrPos=-1;
				int intSegmentLength=-1;

				foreach( string strMarker in lstMarker){
					int intCheckCurr = strText.Substring( intPrevPos).IndexOf( strMarker);
					if( 
						( intCurrPos==-1 && intCheckCurr!=-1)  //initialize the first possible segment digest
						|| ( intCheckCurr!=-1 && intCheckCurr<intCurrPos) //minimize the segment digest
					){
						intCurrPos = intCheckCurr;
						intSegmentLength = strMarker.Length; //set the length of the accepted digest
					}
				}
				
				//if no marker matched or max limit has been reached then truncate at intPrev
				if( intCurrPos==-1 || (intMaxCharCount>0 && intCurrPos+intPrevPos>intMaxCharCount) || (intMaxWordCount>0 && intWordCount>=intMaxWordCount))
					break;
				else{
					intPrevPos=intPrevPos+intCurrPos+intSegmentLength;
					intWordCount++;
				}
			}

			return strText.Substring( 0, intPrevPos).Trim();
		}



		/// <summary>
		/// Checks if string is null, empty, or empty when trimmed.
		/// </summary>
		/// <param name="strValue">String to check if it is generally blank.</param>
		/// <returns>True, if empty, False otherwise.</returns>
		static public bool IsNil( string strValue){
			return strValue==null || strValue.Trim()==String.Empty;
		}


		public static class Filter{
			/// <summary>
			/// Checker function delegate used in filtering text by custom rules.
			/// </summary>
			/// <param name="chValue">Character to be validated.</param>
			/// <returns>True if character is filter-inclusive.</returns>
			public delegate bool FcnCharacterFilter( char chValue);

			/// <summary>
			/// Filter characters from a string value provided the custom filter rule (by providing the custom function matching the delegate).
			/// </summary>
			/// <param name="strValue">Value to be processed by custom character filter rules.</param>
			/// <param name="fcnWhiteListCheck">Custom function acting as the whitelist filter.</param>
			/// <returns></returns>
			public static string FilterText( string strValue, FcnCharacterFilter fcnWhiteListCheck){
				StringBuilder objString = new StringBuilder();

				if( !IsNil( strValue)){
					int intIdx = 0;
					while( intIdx < strValue.Length){
						char chNext = strValue.Substring( intIdx, 1)[0];
						if( fcnWhiteListCheck( chNext))
							objString.Append( strValue.Substring( intIdx, 1));
						intIdx++;
					}
				}
				return objString.ToString();
			}

			/// <summary>
			/// Filter alpha-numeric and space characters from a string value.
			/// </summary>
			/// <param name="strValue">String value to filter.</param>
			/// <returns>Filtered string value.</returns>
			public static string AlphaNumericSpaces( string strValue){
				return FilterText( strValue, CharacterRules.IsAlphaNumericSpaces);
			}


			/// <summary>
			/// Filter alpha-numeric from a string value.
			/// </summary>
			/// <param name="strValue">String value to filter.</param>
			/// <returns>Filtered string value.</returns>
			public static string AlphaNumeric( string strValue){
				return FilterText( strValue, CharacterRules.IsAlphaNumeric);
			}

			public class CharacterRules{
				/// <summary>
				/// Checks if a character is alpha-numeric or space.
				/// </summary>
				/// <param name="chValue">Character to test.</param>
				/// <returns>True if alpha-numeric or space, False otherwise.</returns>
				public static bool IsAlphaNumericSpaces( char chValue){
					return IsAlphaNumeric( chValue) || chValue == ' ';
				}

				/// <summary>
				/// Checks if a character is alpha-numeric.
				/// </summary>
				/// <param name="chValue">Character to test.</param>
				/// <returns></returns>
				public static bool IsAlphaNumeric( char chValue){
					return ( chValue >= 'a' && chValue <= 'z') || ( chValue >= 'A' && chValue <= 'Z') || ( chValue >= '0' && chValue <= '9');
				}
			}
		}
	}
}
