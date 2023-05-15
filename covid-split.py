import csv

input_file = 'covid-data-europe.csv'
output_file = 'cleaned-covid-data-europe.csv'

def update_new_cases(new_cases):
    i = len(new_cases) - 1
    while i >= 0:
        if new_cases[i] > 50:
            week_cases = new_cases[i]
            week_len = 1
            j = i - 1
            while j >= 0 and new_cases[j] == 0:
                week_len += 1
                j -= 1
            daily_cases = round(week_cases / week_len)
            for k in range(j + 1, i + 1):
                new_cases[k] = daily_cases
            i = j
        else:
            i -= 1
    return new_cases

with open(input_file, 'r') as csvfile:
    reader = csv.DictReader(csvfile)
    fieldnames = reader.fieldnames
    data = [row for row in reader]

new_cases = [int(row['new_cases']) if row['new_cases'].isdigit() else 0 for row in data]
updated_new_cases = update_new_cases(new_cases)

for i, row in enumerate(data):
    row['new_cases'] = updated_new_cases[i]

with open(output_file, 'w', newline='') as csvfile:
    writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    writer.writeheader()
    for row in data:
        writer.writerow(row)

print(f"Data has been updated and saved to {output_file}.")
