namespace Yarnique.Payment.API.PaymentProcessing
{
    public class SimpleCreditCardValidator
    {
        public static bool ValidateCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 13 || cardNumber.Length > 19 || !IsAllDigits(cardNumber))
                return false;

            int sum = 0;
            bool doubleDigit = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }

        private static bool IsAllDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
