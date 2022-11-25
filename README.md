# MCA HIS-Demonstrator

This repository contains a demonstrative version of the custom health information system (HIS) 
created within the Mobile Corona Analytic project (MCA).
Other resources, especially publications related to this project, can be found at:

https://doi.org/10.25835/jt0dobsz

*If you analyze, cite or otherwise use this software e.g. in academic research, please always 
include a citation of the relevant paper.*

This software was developed at the [Institute of Microelectronic Systems](https://www.ims.uni-hannover.de/) at the Leibniz University Hannover, Germany.

## Developers

- Christian Fahnemann (christian.fahnemann@ims.uni-hannover.de)
- Christoph Riggers (christoph.riggers@ims.uni-hannover.de)
- Nils Stanislawski (nils.stanislawski@ims.uni-hannover.de)

## Disclaimer

The actual software used differs from the code used in the field and is rather intended to showcase the techniques used.
For this purpose, there is no active database connection but only a file based storage 
which can be used in conjunction with e.g. secure memory devices or network mounts.

Also, only one type of samples and one initial version of the questionnaire are included as examples.
These shall be understood as an example only and may differ from the details discussed in the puplications!

A documentation for the sample ID format used can be found in `HisDemoCode/docs`.

## Prerequisites

The software in this repository requires Microsoft Visual Studio (or compatible) to build and runs on PCs with Microsoft Windows.
To fully test the demonstrator, a handheld barcode scanner (Zebra DS3608) is required, but the main application can run without a scanner.

## Projects

- **Aufnahmestation** is the main testing facility frontend to interview patients and collecting samples.
- **LabelDruckstation** is used to print pre-cut adhesive label sheets used to produce pre-labeled test kits.
- **KitKontrollstation** is used at the end of the test kit production line for quality control (label verification).
- **HisDemoCore** is a common library with functionality used across the application projects.
- **HisDemo.UI** contains commonly used UI elements across the application projects.
- **HisDemo.Test** contains automated unit tests to confirm and validate e.g. the ID encoding scheme.

## Aufnahmestation

In order to run the Aufnahmestation, you need to create two directories to emulate a secure network mounted storage:

	C:\his-demo-data
	C:\his-demo-data\system
	C:\his-demo-data-secondary

After creation, run Aufnahmestation once. 
It will show an error and create a configuration template in the `system` folder created before.
Rename that file to end in `.ini` instead of `.ini.template`. You can change the settings inside it if necessary.

When running the software, it will search for a barcode scanner.
If no compatible scanner is found, it will show a warning but operate normally.

Navigating through the questionnaire pages can be achieved using the TAB and F-keys.

When printing, the default system printer is used and prinout starts immediately.

## LabelDruckstation

Pre-labeled test kits were used to solve two major challenges: 
Ensuring unique pseudonymized sample IDs across all facilities in the project 
and removing the need to print labels anywhere on-site.

For this purpose, pre-cut A4 sized sheets of adhesive labels by Avery Zweckform were used.
The number of labels per sample ID was determined by the requirements of all sample processing steps.
In this repository, the examplary label system for a pharygneal test kit is included, 
which contains of square labels for the test swab and kit bag as well as rectangular
and circular labels for the prepared test tubes and consumables.

Labels must be printed in a careful and coordinated fashion with maticulate tracking of IDs used.
This is crucial to prevent ID duplication which would cause critical errors later on in the field.
A good practice is to keep only one printing station for the whole project and consider any printed label IDs as used.
In doubt, destroy labels and print new ones since the number of possible IDs is unlimited.

## KitKontrollstation

This software is used at the end of the pre-labeled kit production line and **requires the barcode scanner to start**.
It supports quality control by allowing to scan all QR codes within one kit bag and check if all expected sub-types of label are present.
If labels are missing or labels of differend specimen IDs are mixed, an alarm is shown and immediate corrective action needs to be taken.
As with the label printing process, if any doubt is present, destroy the suspect kits and labels.
