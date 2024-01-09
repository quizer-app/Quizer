#!/bin/bash

# Set the source and destination directories
source_dir="/src/images"
destination_dir="/src/wwwroot/images"

# Create the destination directory if it doesn't exist
mkdir -p "$destination_dir"

# Loop through .webp files in the source directory
for webp_file in "$source_dir"/*.webp; do
    if [ -f "$webp_file" ]; then
        # Extract the name (without extension) from the file
        name_without_extension=$(basename "$webp_file" .webp)

        # Create a subdirectory for each file in the destination directory
        subdestination_dir="$destination_dir/$name_without_extension"
        mkdir -p "$subdestination_dir"

        # Move the .webp file to the subdirectory with the name "default.webp"
        mv "$webp_file" "$subdestination_dir/default.webp"

        echo "File '$webp_file' moved to '$subdestination_dir/default.webp'"
    fi
done

echo "Files moved successfully."