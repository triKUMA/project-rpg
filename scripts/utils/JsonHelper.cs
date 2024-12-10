using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

public static class JsonHelper {
  public static T GetPropertyOrDefault<T>(this JObject obj, string propertyName, T defaultVar) {
    var hasProperty = obj.TryGetValue(propertyName, out var property);
    return hasProperty ? property.ToObject<T>() : defaultVar;
  }
}
