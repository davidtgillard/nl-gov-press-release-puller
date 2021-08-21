#!/usr/bin/env bash

OUTPUT_DIR=$1
EXECUTABLE="bin/release/net5.0/NLGovPressReleasePuller.exe"

for YEAR in 1996 1997; do 
	mkdir -p $OUTPUT_DIR/$YEAR
		$EXECUTABLE "https://www.releases.gov.nl.ca/releases/${YEAR}/review${YEAR: -2}.htm" "$OUTPUT_DIR/$YEAR" "weeks/.+\.(htm)|(aspx)" "../\w+/[0-9]{4}n[0-9]{2}\.(htm)|(aspx)"
done

for YEAR in 1998 1999; do 
	mkdir -p $OUTPUT_DIR/$YEAR
		$EXECUTABLE "https://www.releases.gov.nl.ca/releases/review${YEAR: -2}.htm" "$OUTPUT_DIR/$YEAR" "weeks/.+\.(htm)|(aspx)" "../\w+/[0-9]{4}n[0-9]{2}\.(htm)|(aspx)"
done

for YEAR in {2000..2005}; do 
	mkdir -p $OUTPUT_DIR/$YEAR
		$EXECUTABLE "https://www.releases.gov.nl.ca/releases/review${YEAR}.htm" "$OUTPUT_DIR/$YEAR" "weeks/.+\.(htm)|(aspx)" "../\w+/[0-9]{4}n[0-9]{2}\.(htm)|(aspx)"
done

for YEAR in {2006..2012}; do
	mkdir -p $OUTPUT_DIR/$YEAR
		$EXECUTABLE "https://www.releases.gov.nl.ca/releases/${YEAR}/review${YEAR}.htm" "$OUTPUT_DIR/$YEAR" "weeks/.+\.(htm)|(aspx)" "../\w+/[0-9]{4}n[0-9]{2}\.(htm)|(aspx)"
done

for YEAR in {2013..2017}; do
	mkdir -p $OUTPUT_DIR/$YEAR
	$EXECUTABLE "https://www.releases.gov.nl.ca/r/${YEAR}" "$OUTPUT_DIR/$YEAR" "//www.releases.gov.nl.ca/releases/${YEAR}/\w+/[0-9]{4}n[0-9]{2}\.(htm)|(aspx)"
done

for YEAR in {2018..2021}; do
	mkdir -p $OUTPUT_DIR/$YEAR
	$EXECUTABLE "https://www.gov.nl.ca/releases/r/?ny=${YEAR}&nm=&ntype=&ndept=" "$OUTPUT_DIR/$YEAR" "https://www.gov.nl.ca/releases/${YEAR}/\w+/[0-9]{4}n[0-9]{2}"
done





