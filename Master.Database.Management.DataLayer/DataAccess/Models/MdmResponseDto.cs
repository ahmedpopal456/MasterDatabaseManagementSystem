using System;
using Fixit.Core.Database.DataContracts;

namespace Master.Database.Management.DataLayer.DataAccess.Models
{
  public class MdmResponseDto<T> : OperationStatus
  {
    private T _result;
    public MdmResponseDto(bool operationStatus, T resultDto) : base()
    {
      base.IsOperationSuccessful = operationStatus;
      _result = resultDto ?? throw new ArgumentNullException($"{nameof(MdmResponseDto<T>)} expects a value for {nameof(resultDto)}... null argument was provided");
    }

    public T Content { get => _result; set => _result = value; }
  }
}
