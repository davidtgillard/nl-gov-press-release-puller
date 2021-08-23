# nl-gov-press-release-puller
A script to pull all the press releases off gov.nl.ca website and convert them to pdf

## License & Dependencies
The source in this repo is MIT-licensed.

The project uses [iText 7 Community](https://itextpdf.com/en/products/itext-7/itext-7-community), which is licensed under the AGPL.

The project depends upon `dotnet 5` and `bash`.

## How to Use

Build the dotnet assembly by running `dotnet build --configuration Release` from within the root folder.

Then, run the script `get-files.sh`. Edit as required.
