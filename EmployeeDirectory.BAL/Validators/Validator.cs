using System.Text.RegularExpressions;
using System.Net.Mail;
using EmployeeDirectory.BAL.Providers;
using System.Globalization;
using System.Text.Json;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Helper;
using EmployeeDirectory.BAL.Extension;
namespace EmployeeDirectory.BAL.Validators
{
    public class Validator : IValidator
    {
        private readonly IEmpProvider _employee;
        private readonly IGetProjectDeptList _getProjectDeptList;

        public Validator(IEmpProvider emp, IGetProjectDeptList dept)
        {
            _employee = emp;
            _getProjectDeptList= dept;
        }

        private static bool ValidateEmail(string value, string key)
        {
            bool check = ValidateEmptyField(value, key);
            if (check)
            {
                return false;
            }
            try
            {
                MailAddress mail = new(value);
                MessagesInputStore.validationMessages.Remove(key);
                return true;
            }
            catch (FormatException ex)
            {
                MessagesInputStore.validationMessages[key] = $"Email : {ex.Message}";
                return false;
            }
        }

        private static bool ValidatePhone(string input, string key)
        {
            if (input.IsEmpty())
            {
                MessagesInputStore.validationMessages.Remove(key);
                return true;
            }
            if (input.Length != 10)
            {
                MessagesInputStore.validationMessages[key] = "Mobile number should of 10 digit";
                return false;
            }
            bool isDigit = input.All(char.IsDigit);
            if (!isDigit)
            {
                MessagesInputStore.validationMessages[key] = "Mobile number should contains digit only";
                return false;
            }
            else
            {
                MessagesInputStore.validationMessages.Remove(key);
                return true;
            }
        }



        private static bool IsAlphabeticSpace(string input)
        {
            Regex regex = new Regex("^[a-zA-Z ]+$");
            return regex.IsMatch(input);
        }


        public static int ValidateOption(string value)
        {
            try
            {
                int option = Convert.ToInt32(value);
                return option;
            }
            catch (FormatException)
            {
                throw;
            }
        }


        private static bool ValidateEmptyField(string value, string key)
        {
            bool check = value.IsEmpty();
            if (check)
            {
                MessagesInputStore.validationMessages[key] = $"{key} : Required Field can't be null";
            }
            else
            {
                MessagesInputStore.validationMessages.Remove(key);
            }
            return check;
        }


        private static bool IsValidDateFormat(string value, string key)
        {
            if (DateOnly.TryParseExact(value, "dd/MM/yyyy", null, DateTimeStyles.None, out DateOnly result))
            {
                MessagesInputStore.validationMessages.Remove(key);
                return true;
            }
            else
            {
                MessagesInputStore.validationMessages[key] = $"{key} : Date is not in dd/mm/yyyy format";
                return false;
            }
        }

        private static bool ValidateDate(string value, string key)
        {
            if (key.Equals("DOB"))
            {
                if (value.IsEmpty())
                {
                    MessagesInputStore.validationMessages.Remove(key);
                    return true;
                }
                else
                {
                    return IsValidDateFormat(value, key);
                }
            }
            else
            {
                bool check = ValidateEmptyField(value, key);
                if (check)
                {
                    return false;
                }
                else
                {
                    check = IsValidDateFormat(value, key);
                    return check;
                }
            }
        }

        private (bool, string) ValidateRoleName(string value)
        {
            if (!IsAlphabeticSpace(value))
            {
                return (false, "Role name Should contains Alphabets only");
            }
            value = value.Trim().ToLower();
            try
            {
                List<DAL.Models.Role> roles = _employee.GetRoles();
                roles = (from role in roles where role.Name.Equals(value) select role).ToList();
                if (roles.Count > 0)
                {
                    return (false, "This role already exists");
                }
                return (true, "role available");
            }
            catch (JsonException)
            {
                throw;
            }

        }

        private bool ValidateInput(string key, string value, Func<string[]> getStaticData)
        {
            value = value.Trim().ToLower();
            if (value.IsEmpty() && key.Equals("Project"))
            {
                MessagesInputStore.validationMessages.Remove(key);
                return true;
            }
            try
            {
                string[] dataList = getStaticData();
                int index = Array.IndexOf(dataList, value);
                if (index != -1)
                {
                    MessagesInputStore.inputFieldValues[key] = dataList[index];
                    MessagesInputStore.validationMessages.Remove(key);
                    return true;
                }
                else
                {
                    string message = "Selected " + key + " Not Found. Choose from these : - ";
                    foreach (var data in dataList)
                    {
                        message += data.ToString() + ", ";
                    }
                    MessagesInputStore.validationMessages[key] = message;
                    return false;
                }

            }
            catch (JsonException)
            {
                throw;
            }
            catch (FormatException ex)
            {
                MessagesInputStore.validationMessages[key] = $"{key} : {ex.Message}";
                return false;
            }
        }

        public bool ValidateEmployeeInputs(string mode, ref bool isAllInputCorrect)
        {
            bool isAllValid = true;
            foreach (var input in MessagesInputStore.inputFieldValues)
            {
                if (!mode.Equals("Add") && input.Value.IsEmpty())
                {
                    MessagesInputStore.validationMessages.Remove(input.Key);
                }
                else if (MessagesInputStore.validationMessages.ContainsKey(input.Key) || isAllInputCorrect)
                {
                    if (input.Key.Equals("FirstName") || input.Key.Equals("LastName") || input.Key.Equals("Location"))
                    {
                        isAllValid = ValidateEmptyField(input.Value, input.Key) && isAllValid;
                    }
                    else if (input.Key.Equals("Email"))
                    {
                        isAllValid = ValidateEmail(input.Value, input.Key) && isAllValid;
                    }
                    else if (input.Key.Equals("JoinDate") || input.Key.Equals("DOB"))
                    {
                        isAllValid = ValidateDate(input.Value, input.Key) && isAllValid;
                    }
                    else if (input.Key.Equals("JobTitle"))
                    {
                        isAllValid = ValidateInput(input.Key, input.Value, () => _employee.GetRoles().Select(r => r.Name).ToArray()) && isAllValid;
                    }
                    else if (input.Key.Equals("Department"))
                    {
                        isAllValid = ValidateInput(input.Key, input.Value, () => _getProjectDeptList.GetStaticData("Department")) && isAllValid;
                    }
                    else if (input.Key.Equals("Project"))
                    {
                        isAllValid = ValidateInput(input.Key, input.Value, () => _getProjectDeptList.GetStaticData("Project")) && isAllValid;
                    }
                    else if (input.Key.Equals("Mobile"))
                    {
                        isAllValid = ValidatePhone(input.Value, input.Key) && isAllValid;
                    }
                }
            }
            return isAllValid;
        }

        public bool ValidateRoleInputs(ref bool isAllInputCorrect)
        {
            bool isAllValid = true;

            foreach (var input in MessagesInputStore.inputFieldValues)
            {
                if (MessagesInputStore.validationMessages.ContainsKey(input.Key) || isAllInputCorrect)
                {
                    if (input.Key.Equals("Name"))
                    {
                        isAllValid = ValidateEmptyField(input.Value, input.Key) && isAllValid;
                        try
                        {
                            if (!isAllValid)
                            {
                                (isAllValid, string message) = ValidateRoleName(input.Value);
                                if (isAllValid)
                                {
                                    MessagesInputStore.validationMessages.Remove(input.Key);
                                }
                                else
                                {
                                    MessagesInputStore.validationMessages[input.Key] = message;
                                }
                            }
                        }
                        catch (JsonException)
                        {
                            throw;
                        }

                    }
                    else if (input.Key.Equals("Location"))
                    {
                        isAllValid = ValidateEmptyField(input.Value, input.Key) && isAllValid;
                    }
                    else if (input.Key.Equals("Department"))
                    {
                        isAllValid = ValidateInput(input.Key, input.Value, () => _getProjectDeptList.GetStaticData("Department")) && isAllValid;
                    }
                }
            }
            return isAllValid;
        }

    }
}
