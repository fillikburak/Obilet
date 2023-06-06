
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Obilet.Common.Models.Journey;

[JsonObject("data")]
public class JourneyResponseModel
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("partner-id")]
    public int? PartnerId { get; set; }

    [JsonPropertyName("partner-name")]
    public string? PartnerName { get; set; }

    [JsonPropertyName("route-id")]
    public int? RouteId { get; set; }

    [JsonPropertyName("bus-type")]
    public string? BusType { get; set; }

    [JsonPropertyName("bus-type-name")]
    public string? BusTypeName { get; set; }

    [JsonPropertyName("total-seats")]
    public int? TotalSeats { get; set; }

    [JsonPropertyName("available-seats")]
    public int? AvailableSeats { get; set; }
    public Journey? Journey { get; set; }
    public List<Feature>? Features { get; set; }

    [JsonPropertyName("origin-location")]
    public string? OriginLocation { get; set; }

    [JsonPropertyName("destination-location")]
    public string? DestinationLocation { get; set; }

    [JsonPropertyName("is-active")]
    public bool? IsActive { get; set; }

    [JsonPropertyName("origin-location-id")]
    public int? OriginLocationId { get; set; }

    [JsonPropertyName("destination-location-id")]
    public int? DestinationLocationId { get; set; }

    [JsonPropertyName("is-promoted")]
    public bool? IsPromoted { get; set; }

    [JsonPropertyName("cancellation-offset")]
    public int? CancellationOffset { get; set; }

    [JsonPropertyName("has-bus-shuttle")]
    public bool? HasBusShuttle { get; set; }

    [JsonPropertyName("disable-sales-without-gov-id")]
    public bool? DisableSalesWithoutGovId { get; set; }

    [JsonPropertyName("display-offset")]
    public string? DisplayOffset { get; set; }

    [JsonPropertyName("partner-rating")]
    public double? PartnerRating { get; set; }

    [JsonPropertyName("has-dynamic-pricing")]
    public bool? HasDynamicPricing { get; set; }

    [JsonPropertyName("disable-sales-without-hes-code")]
    public bool? DisableSalesWithoutHesCode { get; set; }

    [JsonPropertyName("disable-single-seat-selection")]
    public bool? DisableSingleSeatSelection { get; set; }

    [JsonPropertyName("change-offset")]
    public int? ChangeOffset { get; set; }

    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    [JsonPropertyName("display-coupon-code-input")]
    public bool? DisplayCouponCodeInput { get; set; }

    [JsonPropertyName("disable-sales-without-date-of-birth")]
    public bool? DisableSalesWithoutDateOfBirth { get; set; }

    [JsonPropertyName("open-offset")]
    public int? OpenOffset { get; set; }

    [JsonPropertyName("display-buffer")]
    public object? DisplayBuffer { get; set; }

    [JsonPropertyName("allow-sales-foreign-passenger")]
    public bool? AllowSalesForeignPassenger { get; set; }

    [JsonPropertyName("has-seat-selection")]
    public bool? HasSeatSelection { get; set; }

    [JsonPropertyName("branded-fares")]
    public List<object>? BrandedFares { get; set; }

    [JsonPropertyName("has-gender-selection")]
    public bool? HasGenderSelection { get; set; }

    [JsonPropertyName("has-dynamic-cancellation")]
    public bool? HasDynamicCancellation { get; set; }

    [JsonPropertyName("partner-terms-and-conditions")]
    public object? PartnerTermsAndConditions { get; set; }

    [JsonPropertyName("partner-available-alphabets")]
    public string? PartnerAvailableAlphabets { get; set; }

    [JsonPropertyName("provider-name")]
    public string? ProviderName { get; set; }
}
