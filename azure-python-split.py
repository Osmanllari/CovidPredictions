import pandas as pd

# This function takes a list of new cases and updates it by splitting any week with
# more than 50 cases into daily cases for each day in that week.
def update_new_cases(new_cases):
    i = len(new_cases) - 1
    while i >= 0:
        # Check if the current week has more than 50 cases
        if new_cases[i] > 50:
            week_cases = new_cases[i]
            week_len = 1
            j = i - 1
            # Count the number of consecutive weeks with 0 cases
            while j >= 0 and new_cases[j] == 0:
                week_len += 1
                j -= 1
            # Calculate the daily cases for the week
            daily_cases = round(week_cases / week_len)
            # Update the new_cases list with the daily cases for each day in the week
            for k in range(j + 1, i + 1):
                new_cases[k] = daily_cases
            # Move the index back to the end of the previous week (or the beginning of the list)
            i = j
        else:
            i -= 1
    # Return the updated new_cases list
    return new_cases

# This is the main function that will be called by Azure ML Studio.
def azureml_main(dataframe1: pd.DataFrame, dataframe2: pd.DataFrame = None) -> pd.DataFrame:
    # Copy the input dataframe to avoid modifying the original data.
    data = dataframe1.copy()

    # Convert the 'new_cases' column to numeric data type with errors coerced to NaN.
    data['new_cases'] = pd.to_numeric(data['new_cases'], errors='coerce')

    # Convert the 'new_cases' column to a list and update it using the 'update_new_cases' function.
    new_cases = data['new_cases'].tolist()
    updated_new_cases = update_new_cases(new_cases)

    # Update the 'new_cases' column in the dataframe with the updated values.
    data['new_cases'] = updated_new_cases

    # Return the updated dataframe to Azure ML Studio.
    return data
