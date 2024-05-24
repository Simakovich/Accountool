classDiagram

class AspNetRole {
    +string Id
    +string? Name
    +string? NormalizedName
    +string? ConcurrencyStamp
    +ICollection~AspNetRoleClaim~ AspNetRoleClaims
    +ICollection~AspNetUser~ Users
    +ICollection~AspNetUserRole~ UserRoles
}

class AspNetRoleClaim {
    +int Id
    +string RoleId
    +string? ClaimType
    +string? ClaimValue
    +AspNetRole Role
}

class AspNetUser {
    +string Id
    +string? UserName
    +string? NormalizedUserName
    +string? Email
    +string? NormalizedEmail
    +bool EmailConfirmed
    +string? PasswordHash
    +string? SecurityStamp
    +string? ConcurrencyStamp
    +string? PhoneNumber
    +bool PhoneNumberConfirmed
    +bool TwoFactorEnabled
    +DateTimeOffset? LockoutEnd
    +bool LockoutEnabled
    +int AccessFailedCount
    +ICollection~AspNetUserClaim~ AspNetUserClaims
    +ICollection~AspNetUserLogin~ AspNetUserLogins
    +ICollection~AspNetUserToken~ AspNetUserTokens
    +ICollection~ClientProfile~ ClientProfiles
    +ICollection~Place~ Places
    +ICollection~UserPlace~ UserPlaces
    +ICollection~AspNetRole~ Roles
    +ICollection~AspNetUserRole~ UserRoles
}

class AspNetUserClaim {
    +int Id
    +string UserId
    +string? ClaimType
    +string? ClaimValue
    +AspNetUser User
}

class AspNetUserLogin {
    +string LoginProvider
    +string ProviderKey
    +string? ProviderDisplayName
    +string UserId
    +AspNetUser User
}

class AspNetUserRole {
    +string UserId
    +string RoleId
    +AspNetUser User
    +AspNetRole Role
}

class AspNetUserToken {
    +string UserId
    +string LoginProvider
    +string Name
    +string? Value
    +AspNetUser User
}

class ClientProfile {
    +int Id
    +string Address
    +string? UserId
    +AspNetUser? User
}

class Contract {
    +int Id
    +int OrganizationId
    +int PlaceId
    +int Dogovor
    +int Limit
    +Place Place
    +Organization Organization
}

class Equipment {
    +int Id
    +string ModelEq
    +string Description
    +int PowerEq
    +int PlaceId
    +Place Place
}

class Indication {
    +int Id
    +DateTime Month
    +decimal Value
    +double Tarif1
    +bool Archive
    +int SchetchikId
    +Schetchik Schetchik
}

class PlaceSection {
    +int Id
    +string PlaceName
    +string AdresSection
    +double AreaSection
    +string Kadastr
    +DateTime DataResh
    +string TypeArenda
    +string Certificate
    +DateTime DateArenda
    +ICollection~Place~ Places
}

class MeasureType {
    +int Id
    +string Name
    +ICollection~Schetchik~ Schetchiks
}

class Organization {
    +int Id
    +string Name
    +int Telefon
    +string Email
    +ICollection~Contract~ Contracts
}

class Place {
    +int Id
    +string Name
    +string? ModelPlace
    +DateTime? Arenda
    +string? Address
    +double Area
    +int? TownId
    +int? PlaceSectionId
    +PlaceSection? PlaceSection
    +Town? Town
    +ICollection~Equipment~ Equipments
    +ICollection~Schetchik~ Schetchiks
    +ICollection~UserPlace~ UserPlaces
    +ICollection~Contract~ Contracts
    +ICollection~AspNetUser~ Users
}

class Schetchik {
    +int Id
    +string NomerSchetchika
    +string ModelSchetchika
    +bool TexUchet
    +bool TwoTarif
    +DateTime? Poverka
    +int Poteri
    +int? PlaceId
    +int MeasureTypeId
    +ICollection~Indication~ Indications
    +Place? Place
    +MeasureType MeasureType
}

class Town {
    +int Id
    +string Name
    +ICollection~Place~ Places
}

class UserPlace {
    +string UserId
    +int PlaceId
    +AspNetUser User
    +Place Place
}

class FullIndicationModel {
    +int PlaceId
    +int MeasureTypeId
    +string PlaceName
    +string? TownName
    +string? Address
    +Indication Indication
}

class FirstLastYearModel {
    +DateTime FirstDate
    +DateTime LastDate
}

class IdNameModel {
    +int Id
    +string Name
}

class MeasureWithIndications {
    +IEnumerable~MeasureType~ MeasureTypes
    +PaginatedList~FullIndicationModel~ Indications
    +int MinYear
    +int MaxYear
    +string TitleMeasureTypeName
    +int? TitleMeasureTypeId
    +string SelectedStartYear
    +string SelectedLastYear
    +List~SelectListItem~ Years
    +string SelectedStartMonth
    +string SelectedLastMonth
    +List~SelectListItem~ Months
    +string SelectedPlace
    +List~SelectListItem~ Places
    +string SelectTown
    +List~SelectListItem~ Towns
}

class PaginatedList {
    +int PageIndex
    +int TotalPages
    +bool HasPreviousPage
    +bool HasNextPage
    +CreateAsync(IQueryable~T~, int, int)
}

AspNetRole "1" --* "many" AspNetRoleClaim : Contains
AspNetRoleClaim "*" -- "1" AspNetRole : Belongs To
AspNetRole "1" --* "many" AspNetUserRole : Contains
AspNetUserRole "*" -- "1" AspNetRole : Belongs To

AspNetUser "1" --* "many" AspNetUserClaim : Contains
AspNetUserClaim "*" -- "1" AspNetUser : Belongs To
AspNetUser "1" --* "many" AspNetUserLogin : Contains
AspNetUserLogin "*" -- "1" AspNetUser : Belongs To
AspNetUser "1" --* "many" AspNetUserToken : Contains
AspNetUserToken "*" -- "1" AspNetUser : Belongs To
AspNetUser "1" --* "many" ClientProfile : Contains
ClientProfile "*" -- "1" AspNetUser : Belongs To
AspNetUser "1" --* "many" UserPlace : Contains
UserPlace "*" -- "1" AspNetUser : Belongs To
AspNetUser "1" --* "many" AspNetUserRole : Contains
AspNetUserRole "*" -- "1" AspNetUser : Belongs To

Place "*" -- "many" Equipment : Contains
Equipment "*" -- "1" Place : Belongs To
Place "*" -- "many" Schetchik : Contains
Schetchik "*" -- "1" Place : Belongs To
Place "*" -- "many" UserPlace : Contains
UserPlace "*" -- "1" Place : Belongs To
Place "*" -- "many" Contract : Contains
Contract "*" -- "1" Place : Belongs To

Organization "1" --* "many" Contract : Contains
Contract "*" -- "1" Organization : Belongs To

Schetchik "1" --* "many" Indication : Contains
Indication "*" -- "1" Schetchik : Belongs To

PlaceSection "1" --* "many" Place : Contains
Place "*" -- "1" PlaceSection : Belongs To

MeasureType "1" --* "many" Schetchik : Contains
Schetchik "*" -- "1" MeasureType : Belongs To

Town "1" --* "many" Place : Contains
Place "*" -- "1" Town : Belongs To
