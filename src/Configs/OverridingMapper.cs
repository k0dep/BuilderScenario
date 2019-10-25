namespace BuilderScenario
{
    public class OverridingMapper : IInterpolable
    {
        private object _origin;
        private object _current;

        public OverridingMapper(object current, object origin)
        {
            _origin = origin;
            _current = current;
        }

        public string Interpolate(IConfigMap configMap)
        {
            if (_current is IInterpolable interpolable)
            {
                var result = interpolable.Interpolate(configMap);
                if (result != null)
                {
                    return result;
                }
            }
            
            if (_origin is IInterpolable interpolableCurrent)
            {
                var result = interpolableCurrent.Interpolate(configMap);
                if (result != null)
                {
                    return result;
                }
            }

            return ToString();
        }

        public override string ToString()
        {
            var currentValue = _current?.ToString();
            var originValue = _origin?.ToString();
            
            return currentValue ?? originValue;
        }

        protected bool Equals(OverridingMapper other)
        {
            return Equals(_origin, other._origin) && Equals(_current, other._current);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OverridingMapper) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_origin != null ? _origin.GetHashCode() : 0) * 397) ^ (_current != null ? _current.GetHashCode() : 0);
            }
        }
    }
}