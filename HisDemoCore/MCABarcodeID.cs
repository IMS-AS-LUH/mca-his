using System;

namespace HisDemo
{
    /// <summary>
    /// Prefixing and Suffixing of B32IDs for Barcodes/QR-Codes.
    /// DO NOT START USING IDs WITHOUT PROPER DOCUMENTED ALLOCATION OF SCOPE AND RANGE!
    /// 
    /// Format:
    /// "Prefix-ID-Suffix-Auxiliary"
    /// Prefix is single character.
    /// Separator: Hyphen
    /// ID is B32ID encoded.
    /// Separator: Hyphen
    /// Suffix is 0 to 4 alphanumeric characters .
    /// Optional auxiliary data is separated by another hyphen.
    /// </summary>
    public class MCABarcodeID
    {
        /// <summary>
        /// This is a prefix character which distinguishes differend object classes from another.
        /// These are case insensitive, canonical upper case, alphanumeric single character.
        /// For each scope, there must be one coordinating entity defined.
        /// DO NOT START USING IDs WITHOUT PROPER DOCUMENTED ALLOCATION OF SCOPE AND RANGE!
        /// </summary>
        public enum IDScope : byte
        {
            /// <summary>
            /// None or unknown Prefix (null value).
            /// A dummy ID which can be used for testing purposes.
            /// If encountered in production environment, trigger a warning.
            /// </summary>
            Null = (byte) '0',
            /// <summary>
            /// Identify a distinct speciment (sample).
            /// This is used for prepackaged sampling equipment and 
            /// tracking a single test result between organizational units.
            /// </summary>
            Specimen = (byte) 'S',
            /// <summary>
            /// Identify a distinct report (TBD) - reserved.
            /// </summary>
            Result = (byte) 'R',
            /// <summary>
            /// Identify a parient record.
            /// This can be used to give a patient an ID so they can be recognized later.
            /// </summary>
            Patient = (byte) 'P',
            /// <summary>
            /// Laboratory internal id.
            /// This id scope can be used by the laboratory team to their discretion within the lab process.
            /// Subscopes can be used to further separate ID scopes.
            /// </summary>
            Laboratory = (byte) 'L',
            /// <summary>
            /// Used to identify a distinct employee who is e.g. operating equipment or using the software.
            /// </summary>
            Employee = (byte) 'E',
            /// <summary>
            /// Identifies a hardware/equipment object.
            /// This can be used to ease in inventory tracking, assigning calibration etc. (Future use)
            /// </summary>
            Hardware = (byte) 'H',
            /// <summary>
            /// This is reserved for future use.
            /// Intention: If main scopes are full, subscoping is used to extend scope repertoire. (Future use)
            /// </summary>
            Extended = (byte) 'X',
            /// <summary>
            /// General purpose tracking ID.
            /// </summary>
            Tracking = (byte) 'T',
        }

        /// <summary>
        /// This is the set of allowed characters for use in SubScoping the ID.
        /// </summary>
        public const string AllowedScopingCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Maximum length of the subscope field.
        /// </summary>
        public const int SubScopeMaximumLength = 4;

        /// <summary>
        /// The scope this ID belongs to.
        /// Objects must have unique IDs within a scope except if used with the Extended scope (future use).
        /// DO NOT START USING IDs WITHOUT PROPER DOCUMENTED ALLOCATION OF SCOPE AND RANGE!
        /// </summary>
        public IDScope Scope
        {
            get => _scope;
            set => _scope = SanitizeScope(value);
        }

        /// <summary>
        /// A sub scope that defines a sub-object within the same scope-id-pair.
        /// For example this can be used to identify distinct vials of the same speciment.
        /// In regular scopes, this subscope is hierachically below the scope-id-pair.
        /// For Extended scopes, this is not the case (future use).
        /// </summary>
        public string SubScope
        {
            get => _subscope;
            set => _subscope = SanitizeSubScope(value);
        }

        /// <summary>
        /// The uinque ID within the scope.
        /// Objects must have unique IDs within a scope except if used with the Extended scope (future use).
        /// DO NOT START USING IDs WITHOUT PROPER DOCUMENTED ALLOCATION OF SCOPE AND RANGE!
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Optional additional information encoded in the barcode (especially QR Codes).
        /// This is scoped by the scope-id-subscope triplet.
        /// This data is explicitly optional. It MUST NOT be required to render the ID unique.
        /// At any point within the process, this auxiliary information may be dropped, 
        /// resulting in a still valid unique ID.
        /// Examples for Auxiliary use:
        /// - Token information for authentication purposes (e.g. Patient ID + Access Token).
        /// - Copy of laboratory information (e.g. to transfer data via QR-Code).
        /// For any distinct use of this auxiliary field, define a proper subscope!
        /// </summary>
        public string Auxiliary { get; set; }

        /// <summary>
        /// Access the ID field in B32 encoded format.
        /// </summary>
        public string B32ID
        {
            get => HisDemo.B32ID.Encode(ID);
            set
            {
                try
                {
                    ID = HisDemo.B32ID.Decode(value);
                } catch (HisDemo.B32ID.DecodingException)
                {
                    throw new InvalidB32IDException();
                }
            }
        }

        /// <summary>
        /// Access the full barcode representation string.
        /// Format: Scope-ID-Subscope-Auxiliary
        /// With Subscope and Auxiliary being optional.
        /// Note: If no subscope, two consecutive hyphens are used!
        /// </summary>
        public string Value
        {
            get => Encode(true);
            set => Decode(value, true);
        }

        /// <summary>
        /// Access the barcode representation string without auxiliary data.
        /// Format: Scope-ID-Subscope
        /// Does not remove the auxiliary data from the object.
        /// Does not accept values including auxiliary field.
        /// </summary>
        public string ScopedIDValue
        {
            get => Encode(false);
            set => Decode(value, false);
        }

        /// <summary>
        /// Create an empty object.
        /// </summary>
        public MCABarcodeID() { }
        /// <summary>
        /// Create an object based on the given encoded data.
        /// </summary>
        /// <param name="value">Scope-ID-Subscope-Auxiliary representation</param>
        /// <exception cref="InvalidMCABarcodeIDException">Thrown if invalid value given.</exception>
        public MCABarcodeID(string value)
        {
            Decode(value);
        }
        /// <summary>
        /// Create an object based on the given fields.
        /// </summary>
        /// <param name="scope">The scope of this ID.</param>
        /// <param name="id">The numeric ID value.</param>
        /// <param name="subscope">Optional subscope.</param>
        /// <param name="auxiliary">Optional auxiliary data.</param>
        public MCABarcodeID(IDScope scope, uint id, string subscope = null, string auxiliary = null)
        {
            Scope = scope;
            ID = id;
            SubScope = subscope;
            Auxiliary = auxiliary;
        }

        /// <summary>
        /// Clear the auxiliary field data.
        /// </summary>
        public void ClearAuxiliary()
        {
            Auxiliary = null;
        }

        /// <summary>
        /// Get the string representation of this objects value.
        /// </summary>
        /// <returns>The Value field.</returns>
        public override string ToString()
        {
            return Encode();
        }

        /// <summary>
        /// Checks for canonical equality of all values.
        /// </summary>
        /// <param name="obj">The MCABarcodeID to compare to.</param>
        /// <returns>True if scope, ID, subscope and auxiliary match.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is MCABarcodeID)
            {
                MCABarcodeID b = (MCABarcodeID)obj;
                return b._scope == _scope &&
                    b.ID == ID &&
                    b.SubScope == _subscope &&
                    b.Auxiliary == Auxiliary;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks for canonical equality based on Scope-ID-Subscope. Does NOT compare auxiliary data!
        /// </summary>
        /// <param name="b">The MCABarcodeID to compare to.</param>
        /// <returns>True if scope, ID and subscope match.</returns>
        public bool EqualScopedID(MCABarcodeID b)
        {
            if (b != null)
            {
                return b._scope == _scope &&
                    b.ID == ID &&
                    b.SubScope == _subscope;
            }
            else
                return false;
        }

        private IDScope _scope = IDScope.Null;
        private string _subscope = "";

        private string Encode(bool includeAuxiliary = true)
        {
            string s = $"{(char)(byte)_scope}-{B32ID}";
            if (_subscope != null || (Auxiliary != null && includeAuxiliary))
                s += $"-{_subscope ?? ""}";
            if (Auxiliary != null && includeAuxiliary)
                s += $"-{Auxiliary}";
            return s;
        }

        private static char[] separatorArray = { '-' };

        private void Decode(string str, bool allowAuxiliary = true)
        {
            string[] parts = str.Split(separatorArray, 4);
            if (parts.Length < 2 || parts[0].Length != 1)
                throw new InvalidMCABarcodeIDException();
            if (!allowAuxiliary && parts.Length > 3)
                throw new InvalidMCABarcodeIDException();
            _scope = SanitizeScope(parts[0][0]);
            B32ID = parts[1];
            if (parts.Length >= 3)
                SubScope = parts[2];
            else
                SubScope = null;
            if (parts.Length == 4)
                Auxiliary = parts[3];
            else
                Auxiliary = null;
        }

        /// <summary>
        /// Ensure scope is known and valid.
        /// </summary>
        /// <param name="scope">The candidate scope.</param>
        /// <returns>The valid scope.</returns>
        /// <exception cref="InvalidIDScopeException">Thrown if input is not valid.</exception>
        public static IDScope SanitizeScope(IDScope scope)
        {
            if (!Enum.IsDefined(typeof(IDScope), scope))
                throw new InvalidIDScopeException();
            return scope;
        }
        /// <summary>
        /// Ensure a byte identifying the scope is known and valid. Converts to proper enum type.
        /// </summary>
        /// <param name="scope">The candidate scope as byte.</param>
        /// <returns>The converted valid scope.</returns>
        /// <exception cref="InvalidIDScopeException">Thrown if input is not valid.</exception>
        public static IDScope SanitizeScope(byte scope)
        {
            if (!Enum.IsDefined(typeof(IDScope), scope))
                throw new InvalidIDScopeException();
            return (IDScope) scope;
        }
        /// <summary>
        /// Ensure a character identifying the scope is known and valid. Converts to proper enum type.
        /// </summary>
        /// <param name="scope">The candidate scope as character.</param>
        /// <returns>The converted valid scope.</returns>
        /// <exception cref="InvalidIDScopeException">Thrown if input is not valid.</exception>
        public static IDScope SanitizeScope(char scope)
        {
            if (AllowedScopingCharacters.IndexOf(scope) < 0)
                throw new InvalidIDScopeException();
            return SanitizeScope((byte)scope);
        }
        /// <summary>
        /// Ensure a given subscope string is valid and canonicalizes it.
        /// </summary>
        /// <param name="subscope">The subscope candidate string.</param>
        /// <returns>The valid subscope in canonical form.</returns>
        /// <exception cref="InvalidIDSubScopeException">Thrown if input is not valid.</exception>
        public static string SanitizeSubScope(string subscope)
        {
            if (subscope == null)
                return null;
            if (subscope.Length > SubScopeMaximumLength)
                throw new InvalidIDSubScopeException();
            subscope = subscope.ToUpperInvariant();
            foreach (char c in subscope)
            {
                if (AllowedScopingCharacters.IndexOf(c) < 0)
                    throw new InvalidIDSubScopeException();
            }
            return subscope;
        }

        /// <summary>
        /// Indicates that an invalid barcode ID encoding was presented or that a field given was invalid.
        /// All regular MCABarcodeID related exceptions are based on this exception, so it can be used to catch them all.
        /// </summary>
        public class InvalidMCABarcodeIDException : Exception { };

        /// <summary>
        /// Specific exception indicating an invalid scope given in a barcode or as a field/property value.
        /// </summary>
        public class InvalidIDScopeException : InvalidMCABarcodeIDException { };

        /// <summary>
        /// Specific exception indicating an invalid subcope given in a barcode or as a field/property value.
        /// </summary>
        public class InvalidIDSubScopeException : InvalidMCABarcodeIDException { };

        /// <summary>
        /// Specific exception indicating an invalid B32ID given in a barcode or as a field/property value.
        /// This encapsulates B32ID decoding errors so only <c>InvalidMCABarcodeIDException</c> is required to catch regular exceptions from this class.
        /// </summary>
        public class InvalidB32IDException : InvalidMCABarcodeIDException { };
    }
}
