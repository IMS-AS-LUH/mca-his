# Consistent scoping for barcode identification

Within the project, differend organizational units may require multiple kinds of IDs to
distinguish various objects. While an operator can cleary differentiate a person from a
specimen vial, the barcode reader can not. To aid in automation and error prevention,
we choose a prefix-suffix-scheme to decorate the B32IDs used for object identification.
This enables software to clearly associate a scanned code with the proper usage and
fill in the correct field, check the operator scanned the correct object etc.

## Scoping hierarchy

A numeric ID always belongs to a scope (class of objects). For example, IDs for patients
belong to a different scope than specimen IDs or employee IDs.

The pair of a scope and ID must always uniquely identify an object. Often however, there are 
sub-objects or aspects that relate so closely, that it does not make sense to separate
them. For example: The vials within a specimen test kit belong to one single human specimen
troughout processing. Operators need to associate them by printed B32ID in the laboratory.
Thus, all one-time-use items and labels associating the same specimen shall carry the same
scope-ID-pair. If they need to be distinguished by software, subscoping is used.
See below ("Subscoping for Specimen kits") for example definitions.

This yields the following hierarchy:

	Scope -> ID -> SubScope

For obvious reasons like natural sorting, the combined Barcode value will contain these
parts in the same order.
Note that codes with the same ID but a different scope have nothing to do with eachother.

## Concatenation and allowed Symbols

To enable rendering the scoped ID with commonly used bacrode systems, we restrict the available
symbols to alphanumeric characters which are canonically uppercase. This means that the ID is
case insensitive but shall always be converted to uppercase for display/storage/comparison.

The scope field is a single character. If we ever run out of scoping characters (we have 36 in theory),
the 'X' (Extended) scope can be used (see below).

The subscope field can be empty or contain up to 4 alphanumeric characters (yields 36^4 in theory).

## Display to operators

In order for operators to more easily read and associate IDs, the scope and subscope shall not be
printed on labels by default. If space is sufficient, print the concatenated form exactly as it is
used for the barcode (e.g. "S-AAAA-VT03"). The label shall contain primarily a big, readable ID
(e.g. here "AAAA") and preferrably a small indication of the scope (e.g. "MCA Specimen", "MCA Sp.").

## Auxiliary data

Especially QRCodes can contain a large amount of data. Since there are use cases to deliver a
scoped ID with some auxiliary data, we define the option to append auxiliary data to the fully subscoped id,
separated by another hyphen.

By definition, this auxiliary data must not be required to render the ID unique. It is intended
to carry additional information like authentication tokens, laboratory result data, encrypted patient recall information, etc.
At any stage in data transfer, this field may be dropped and must not cause loss of identification
quality by this. 

There is no restriction on the auxiliary data content. It can be chosen to match the used barcode format
and is internally represented as a string. The character scope should be restricted to ASCII.
Auxiliary data is not case-converted during transmission, storage etc. - in contrast to the scoped ID itselt.

The use of auxiliary data should be connected to one or more appropriate scope-subscope-pairs so
it can be processed correctly.

## Scopes

Currently, the following scopes are defined. Some are for future use. Any unused allowed character is
to be considered reserved! Do not add your own scopes without proper allocation with the core IT team!

	0 : None/Unknown/Dummy - Can be used for dummy labels and testing purposes.
	A : Alias for pseudomization of Patiens ("Alias")
	U : Alias for pseudomization of Specimen ("Untersuchung")
	S : Specimen - Identify a distinct speciment (sample). 
		This is used for prepackaged sampling equipment and tracking a single test 
		result between organizational units.
    R : Result - TBD (reserved)
	P : Patient - Identify a parient record. This can be used to give a patient an 
	    ID so they can be recognized later.
	L : Laboratory - Laboratory internal id. This id scope can be used by the laboratory 
	    team to their discretion within the lab process. Subscopes can be used to further 
		separate ID scopes.
	E : Employee - Used to identify a distinct employee who is e.g. operating 
	    equipment or using the software.
	H : Hardware - Identifies a hardware/equipment object.
	    This can be used to ease in inventory tracking, assigning calibration etc. (Future use)
	X : Extended - This is reserved for future use. Intention: If main scopes are full, 
	    subscoping is used to extend scope repertoire. (Future use)
	T : Tracking for general purpose.
	    The main scope does not imply anything specific except that it is not an ID of another scope.
		The subscope is used to distinguish, in which context the code is used.

### Subscoping for Specimen kits

The 'S' Scope (for Specimen) uses subscoping to distinguish different types of vials/labels.
This can be used in kit packing verification tests to cross-check the number of physically distinct
labels scanned. For example, the following subscopes could be defined:

	ID Zero ("AAAA") is defined as null/"not taken"-Placeholder

	; First Kit Type for Pharygneal-Test (label color: White)
	* nn means a numeric value from 0 to 99 - ANY OTHER also reserved for this!
	* SPECIAL: nn=21 is a special inverted label for second preparation product (out of container)
	KTnn : Kit label, e.g. for transfer swab tube, packaging unit (bag), general identification
	VSnn : Vial side label, e.g. around body of Eppi, Quia etc.
	VTnn : Vial top label, e.g. on top of lid for Eppi, Quia etc.

	; Big-QR Scopes
	PRLn : Preliminary Software Reference Label - For patient printout

All others are for now reserved.

NOTE: The scoping information herein is solely for demonstrational purposes and shall not represent the actual system used in the project!
