

namespace WPFTest.Data
{ 
    public class ViewModelRequest
    {
        public Type ViewModelType { get; }
        public object[] Parameters { get; }

        public ViewModelRequest(Type viewModelType, params object[] parameters)
        {
            ViewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
            Parameters = parameters;
        }

        public override bool Equals(object obj)
        {
            if (obj is ViewModelRequest other)
            {
                if (ViewModelType != other.ViewModelType)
                    return false;

                if (Parameters == null && other.Parameters == null)
                    return true;

                if (Parameters == null || other.Parameters == null)
                    return false;

                if (Parameters.Length != other.Parameters.Length)
                    return false;

                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (!Equals(Parameters[i], other.Parameters[i]))
                        return false;
                }

                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ViewModelType.GetHashCode();
                if (Parameters != null)
                {
                    foreach (var param in Parameters)
                    {
                        hash = hash * 23 + (param?.GetHashCode() ?? 0);
                    }
                }
                return hash;
            }
        }

        public static bool operator ==(ViewModelRequest left, ViewModelRequest right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ViewModelRequest left, ViewModelRequest right)
        {
            return !Equals(left, right);
        }
    }
}
