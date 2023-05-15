import csv

input_file = 'owid-covid-data.csv'
output_file = 'cleaned-covid-data.csv'

with open(input_file, 'r') as csvfile:
    reader = csv.DictReader(csvfile)
    fieldnames = reader.fieldnames
    data = [row for row in reader if row['continent'] == 'Europe']

with open(output_file, 'w', newline='') as csvfile:
    writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    writer.writeheader()
    for row in data:
        writer.writerow(row)

print(f"Data has been cleaned and saved to {output_file}.")
