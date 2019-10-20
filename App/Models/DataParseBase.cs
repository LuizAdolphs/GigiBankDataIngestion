namespace GigiBankDataIngestion.Models
{
    using GigiBankDataIngestion.Common;

    public abstract class DataParseBase<T> : DataBase where T : DataBase
    {
        protected const char separator = 'ç';

        public Response<T> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new Response<T>(false)
                    .AddErrorMessage("Is not allowed to add empty data");
            }

            return this.ParseEngine(data);
        }
        protected abstract Response<T> ParseEngine(string data);
    }
}
