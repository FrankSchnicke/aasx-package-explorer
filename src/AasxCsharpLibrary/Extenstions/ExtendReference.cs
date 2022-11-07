﻿using AasCore.Aas3_0_RC02;
using AdminShellNS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Extenstions
{
    public static class ExtendReference
    {
        #region AasxPackageExplorer

        public static bool IsValid(this Reference reference)
        {
            return reference.Keys == null || reference.Keys.IsEmpty();
        }

        //This is alternative for operator overloding method +, as operator overloading cannot be done in extension classes
        public static Reference Add(this Reference a, Reference b)
        {
            a.Keys?.AddRange(b?.Keys);
            return a;
        }

        public static bool IsEmpty(this Reference reference)
        {
            if (reference == null || reference.Keys == null || reference.Keys.Count < 1)
            {
                return true;
            }

            return false;
        } 

        #endregion

        public static bool Matches (this Reference reference, KeyTypes keyType, string id, MatchMode matchMode = MatchMode.Strict)
        {
            if(reference.IsEmpty())
            {
                return false;
            }

            if(reference.Keys.Count == 1)
            {
                var key = reference.Keys[0];
                return key.Matches(new Key(keyType, id), matchMode);
            }

            return false;
        }
        public static bool Matches(this Reference reference, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            if (reference.Keys.Count == 1) // As per old implementation
            {
                if (reference.Keys[0].Value == id)
                    return true;
            }

            return false;
        }

        public static bool Matches(this Reference reference, Reference otherReference, MatchMode matchMode = MatchMode.Strict)
        {
            if (reference.Keys == null || reference.Keys.Count == 0 || otherReference.Keys == null || otherReference.Keys.Count == 0)
            {
                return false;
            }

            bool match = true;
            for (int i = 0; i < reference.Keys.Count; i++)
            {
                match = match && reference.Keys[i].Matches(otherReference.Keys[i], matchMode);
            }

            return match;
        }

        public static bool MatchesExactlyOneKey(this Reference reference, Key key, MatchMode matchMode = MatchMode.Strict)
        {
            if(key == null || reference.Keys == null || reference.Keys.Count != 1)
            {
                return false;
            }

            var referenceKey = reference.Keys[0];
            return referenceKey.Matches(key, matchMode);
        }

        public static string GetAsIdentifier(this Reference reference)
        {
            if (reference.Type == ReferenceTypes.GlobalReference) // Applying only to Global Reference, based on older implementation, TODO:Make it Generic
            {
                if (reference.Keys == null || reference.Keys.Count < 1)
                {
                    return null;
                }

                return reference.Keys[0].Value;
            }

            return null;
        }

        public static string MostSignificantInfo(this Reference reference)
        {
            if (reference.Keys.Count < 1)
            {
                return "-";
            }

            var i = reference.Keys.Count - 1;
            var output = reference.Keys[i].Value;
            if (reference.Keys[i].Type == KeyTypes.FragmentReference && i > 0)
                output += reference.Keys[i - 1].Value;
            return output;
        }

        public static Key GetAsExactlyOneKey(this Reference reference)
        {
            if (reference.Keys == null || reference.Keys.Count != 1)
            {
                return null;
            }

            var key = reference.Keys[0];
            return new Key(key.Type, key.Value);
        }

        public static string ToStringExtended( this Reference reference, string delimiter = ",")
        {
            if(reference.Keys == null)
            {
                throw new NullValueException("Keys");
            }

            return reference.Keys.ToStringExtended(delimiter);
        }


    }

}