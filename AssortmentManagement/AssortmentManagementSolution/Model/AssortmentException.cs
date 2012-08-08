using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssortmentManagement.Model
{
    public class AssortmentException : Exception
    {

        public AssortmentException(string message)
            : base(message)
        {
        }
    }

    public class CancelException : AssortmentException
    {
        public CancelException(string message)
            : base(message)
        {

        }
    }

    public class AssortmentDBException : AssortmentException
    {
        public AssortmentDBException(string message)
            : base(message)
        {

        }
    }

    public class LocListNullException : AssortmentException
    {
        public LocListNullException (string message)
            : base(message)
        {
            
        }
    }

    public class AssortmentLayoutNullException : AssortmentException
    {
        public AssortmentLayoutNullException(string message)
            : base(message)
        {

        }
    }

    public class AssortmentDescNullException : AssortmentException
    {
        public AssortmentDescNullException(string message)
            : base(message)
        {

        }
    }

    public class ConnectionException : AssortmentException
    {
        public ConnectionException(string message)
            : base(message)
        {
        }
    }

    public class AuthorizationException : AssortmentException
    {
        public AuthorizationException(string message)
            : base(message)
        {
        }
    }

    public class DocumentNotExistsException : AssortmentException
    {
        public DocumentNotExistsException(string message):base(message)
        {
            
        }
    }
}
