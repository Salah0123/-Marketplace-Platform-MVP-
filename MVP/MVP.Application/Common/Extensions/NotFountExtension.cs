namespace MVP.Application.Common.Extensions;

public class NotFountExtension(string entity, int id) : Exception($"The {entity} with ID {id} was not found.")
{
}
