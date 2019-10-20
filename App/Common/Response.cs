namespace GigiBankDataIngestion.Common
{
    using System.Collections.Generic;

    public class Response<T> where T : class
    {
        private List<string> messageList = new List<string>();

        private T? lastAddedModel;

        internal Response(bool isValid = false) => this.IsValid = isValid;
        public bool IsValid { get; private set; } = false;

        public Response<T> AddErrorMessage(string message)
        {
            this.IsValid = false;

            this.messageList.Add(message);

            return this;
        }

        public Response<T> AddObjectModel(T? model)
        {
            this.lastAddedModel = model;

            return this;
        }

        public T? Model => this.lastAddedModel;

        public string[] MessageList => this.messageList.ToArray();
    }
}