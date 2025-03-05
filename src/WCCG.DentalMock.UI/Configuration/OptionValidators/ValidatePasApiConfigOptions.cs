using Microsoft.Extensions.Options;

namespace WCCG.DentalMock.UI.Configuration.OptionValidators;

[OptionsValidator]
public partial class ValidateEReferralsApiConfigOptions : IValidateOptions<EReferralsApiConfig>;
