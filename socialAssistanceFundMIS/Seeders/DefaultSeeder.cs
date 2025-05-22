using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Bogus;

namespace socialAssistanceFundMIS.Seeders
{
    public static class DefaultSeeder
    {
        public static void Run(ApplicationDbContext context)
        {
            if (!context.GeographicLocationTypes.Any())
            {
                var locationTypes = new List<GeographicLocationType>
                    {
                        new() { Name = "County", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Sub-County", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Location", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Sub-Location", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Village", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };

                context.GeographicLocationTypes.AddRange(locationTypes);
                context.SaveChanges();
            }

            if (!context.GeographicLocations.Any())
                {
                                // Counties
                    var counties = new List<GeographicLocation>
                    {
                        new() { Name = "Nairobi", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 1
                        new() { Name = "Kisumu", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 2
                        new() { Name = "Mombasa", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // ID = 3
                        new() { Name = "Kiambu", GeographicLocationTypeId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow } // ID = 4
                    };
                    context.GeographicLocations.AddRange(counties);
                    context.SaveChanges();

                                // Sub-Counties
                    var subCounties = new List<GeographicLocation>
                    {
                        new() { Name = "Westlands", GeographicLocationTypeId = 2, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, // Nairobi
                        new() { Name = "Lang'ata", GeographicLocationTypeId = 2, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Kisumu Central", GeographicLocationTypeId = 2, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Nyali", GeographicLocationTypeId = 2, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Thika Town", GeographicLocationTypeId = 2, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(subCounties);
                    context.SaveChanges();

                                // Locations
                    var locations = new List<GeographicLocation>
                    {
                        new() { Name = "Parklands", GeographicLocationTypeId = 3, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen", GeographicLocationTypeId = 3, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Milimani", GeographicLocationTypeId = 3, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani", GeographicLocationTypeId = 3, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa", GeographicLocationTypeId = 3, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(locations);
                    context.SaveChanges();

                                // Sub-Locations
                    var subLocations = new List<GeographicLocation>
                    {
                        new() { Name = "Parklands North", GeographicLocationTypeId = 4, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen South", GeographicLocationTypeId = 4, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Milimani East", GeographicLocationTypeId = 4, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani North", GeographicLocationTypeId = 4, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa West", GeographicLocationTypeId = 4, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(subLocations);
                    context.SaveChanges();

                    // Villages
                    var villages = new List<GeographicLocation>
                    {
                        new() { Name = "Kipro Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Karen Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Kisumu Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Mkomani Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new() { Name = "Ngoigwa Village", GeographicLocationTypeId = 5, GeographicLocationParentId = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    };
                    context.GeographicLocations.AddRange(villages);
                    context.SaveChanges();
                }


            if (!context.AssistancePrograms.Any())
            {
                var assistancePrograms = new List<AssistanceProgram>
                {
                    new() { Name = "Orphans and vulnerable children", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Poor elderly persons", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Persons with disability", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Persons in extreme poverty", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Any other", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.AssistancePrograms.AddRange(assistancePrograms);
                context.SaveChanges();
            }

            // Designations
            if (!context.Designations.Any())
            {
                var designations = new List<Designation>
                {
                    new() { Name = "Manager", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Field Officer", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Designations.AddRange(designations);
                context.SaveChanges();
            }

            // Marital Status
            if (!context.MaritalStatuses.Any())
            {
                var maritalStatuses = new List<MaritalStatus>
                {
                    new() { Name = "Single", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Married", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Divorced", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.MaritalStatuses.AddRange(maritalStatuses);
                context.SaveChanges();
            }

            // Officers
            if (!context.Officers.Any())
            {
                // Retrieve the existing designations
                var managerDesignation = context.Designations.FirstOrDefault(d => d.Name == "Manager");
                var supervisorDesignation = context.Designations.FirstOrDefault(d => d.Name == "Field Officer");

                // Create officers and associate with designations
                var officers = new List<Officer>
        {
            new Officer
            {
                FirstName = "John",
                MiddleName = "Doe",
                LastName = "Smith",
                DesignationId = managerDesignation.Id,  // Assign Manager designation
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Officer
            {
                FirstName = "Jane",
                MiddleName = "Anne",
                LastName = "Doe",
                DesignationId = supervisorDesignation.Id,  // Assign Supervisor designation
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

                // Add officers to the context and save changes
                context.Officers.AddRange(officers);
                context.SaveChanges();
            }

            if (!context.OfficialRecords.Any())
            {
                var officers = context.Officers.ToList();
                var faker = new Faker("en");

                var officialRecords = new List<OfficialRecord>();

                var count = 5;

                for (int i = 0; i < count; i++)
                {
                    var officer = faker.PickRandom(officers);

                    officialRecords.Add(new OfficialRecord
                    {
                        OfficerId = officer.Id,
                        OfficiationDate = faker.Date.Past(2),
                        Removed = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                context.OfficialRecords.AddRange(officialRecords);
                context.SaveChanges();
            }

            // Phone Number Types
            if (!context.PhoneNumberTypes.Any())
            {
                var phoneNumberTypes = new List<PhoneNumberType>
                {
                    new() { Name = "Mobile", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Landline", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.PhoneNumberTypes.AddRange(phoneNumberTypes);
                context.SaveChanges();
            }

            // Sex
            if (!context.Sexes.Any())
            {
                var sexes = new List<Sex>
                {
                    new() { Name = "Male", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Female", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Other", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Sexes.AddRange(sexes);
                context.SaveChanges();
            }

            // Status
            if (!context.Statuses.Any())
            {
                var statuses = new List<Status>
                {
                    new() { Name = "Pending", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new() { Name = "Approved", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            // Applicants
            if (!context.Applicants.Any())
            {
                var maleSex = context.Sexes.FirstOrDefault(s => s.Name == "Male");
                var femaleSex = context.Sexes.FirstOrDefault(s => s.Name == "Female");

                var single = context.MaritalStatuses.FirstOrDefault(m => m.Name == "Single");
                var married = context.MaritalStatuses.FirstOrDefault(m => m.Name == "Married");

                var village1 = context.GeographicLocations.FirstOrDefault(g => g.Name == "Kipro Village");
                var village2 = context.GeographicLocations.FirstOrDefault(g => g.Name == "Karen Village");

                var applicants = new List<Applicant>
                {
                    new()
                    {
                        FirstName = "Alex",
                        MiddleName = "James",
                        LastName = "Otieno",
                        Email = "alex@email.com",
                        SexId = maleSex.Id,
                        Dob = new DateOnly(1990, 5, 15),
                        MaritialStatusId = single.Id,
                        IdentityCardNumber = "MW99012345",
                        VillageId = village1.Id,
                        PhysicalAddress = "Area 47, Lilongwe",
                        PostalAddress = "P.O. Box 100",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        FirstName = "Beatrice",
                        MiddleName = "Wanjiku",
                        LastName = "Mwangi",
                        Email = "beat@email.com",
                        SexId = femaleSex.Id,
                        Dob = new DateOnly(1990, 5, 15),
                        MaritialStatusId = married.Id,
                        IdentityCardNumber = "MW99012345",
                        VillageId = village2.Id,
                        PhysicalAddress = "Area 47, Lilongwe",
                        PostalAddress = "P.O. Box 100",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        FirstName = "Charles",
                        MiddleName = "Kamau",
                        LastName = "Kariuki",
                        Email = "charles@email.com",
                        SexId = maleSex.Id,
                        Dob = new DateOnly(1990, 5, 15),
                        MaritialStatusId = single.Id,
                        IdentityCardNumber = "MW99012345",
                        VillageId = village1.Id,
                        PhysicalAddress = "Area 47, Lilongwe",
                        PostalAddress = "P.O. Box 100",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        FirstName = "Diana",
                        MiddleName = "Atieno",
                        LastName = "Owino",
                        Email = "diana@email.com",
                        SexId = femaleSex.Id,
                        Dob = new DateOnly(1990, 5, 15),
                        MaritialStatusId = married.Id,
                        IdentityCardNumber = "MW99012345",
                        VillageId = village2.Id,
                        PhysicalAddress = "Area 47, Lilongwe",
                        PostalAddress = "P.O. Box 100",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        FirstName = "Elijah",
                        MiddleName = "Kiptoo",
                        LastName = "Koech",
                        Email = "elijah@email.com",
                        SexId = maleSex.Id,
                        Dob = new DateOnly(1990, 5, 15),
                        MaritialStatusId = single.Id,
                        IdentityCardNumber = "MW99012345",
                        VillageId = village1.Id,
                        PhysicalAddress = "Area 47, Lilongwe",
                        PostalAddress = "P.O. Box 100",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.Applicants.AddRange(applicants);
                context.SaveChanges();
            }


            // Phone Numbers for Applicants
            if (!context.ApplicantPhoneNumbers.Any())
            {
                var applicant = context.Applicants.Find(1);
                var mobileType = context.PhoneNumberTypes.First(p => p.Name == "Mobile");
                var landlineType = context.PhoneNumberTypes.First(p => p.Name == "Landline");

                var phoneNumbers = new List<ApplicantPhoneNumber>
                {
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = mobileType.Id,
                        PhoneNumber = "+254712345678",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = landlineType.Id,
                        PhoneNumber = "0201234567",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.ApplicantPhoneNumbers.AddRange(phoneNumbers);
                context.SaveChanges();
            }


            if (!context.ApplicantPhoneNumbers.Any())
            {
                var applicant = context.Applicants.Find(2);
                var mobileType = context.PhoneNumberTypes.First(p => p.Name == "Mobile");
                var landlineType = context.PhoneNumberTypes.First(p => p.Name == "Landline");

                var phoneNumbers = new List<ApplicantPhoneNumber>
                {
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = mobileType.Id,
                        PhoneNumber = "+254712345678",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = landlineType.Id,
                        PhoneNumber = "0201234567",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.ApplicantPhoneNumbers.AddRange(phoneNumbers);
                context.SaveChanges();
            }

            if (!context.ApplicantPhoneNumbers.Any())
            {
                var applicant = context.Applicants.Find(3);
                var mobileType = context.PhoneNumberTypes.First(p => p.Name == "Mobile");
                var landlineType = context.PhoneNumberTypes.First(p => p.Name == "Landline");

                var phoneNumbers = new List<ApplicantPhoneNumber>
                {
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = mobileType.Id,
                        PhoneNumber = "+254712345678",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = landlineType.Id,
                        PhoneNumber = "0201234567",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.ApplicantPhoneNumbers.AddRange(phoneNumbers);
                context.SaveChanges();
            }

            if (!context.ApplicantPhoneNumbers.Any())
            {
                var applicant = context.Applicants.Find(4);
                var mobileType = context.PhoneNumberTypes.First(p => p.Name == "Mobile");
                var landlineType = context.PhoneNumberTypes.First(p => p.Name == "Landline");

                var phoneNumbers = new List<ApplicantPhoneNumber>
                {
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = mobileType.Id,
                        PhoneNumber = "+254712345678",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = landlineType.Id,
                        PhoneNumber = "0201234567",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.ApplicantPhoneNumbers.AddRange(phoneNumbers);
                context.SaveChanges();
            }

            if (!context.ApplicantPhoneNumbers.Any())
            {
                var applicant = context.Applicants.Find(5);
                var mobileType = context.PhoneNumberTypes.First(p => p.Name == "Mobile");
                var landlineType = context.PhoneNumberTypes.First(p => p.Name == "Landline");

                var phoneNumbers = new List<ApplicantPhoneNumber>
                {
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = mobileType.Id,
                        PhoneNumber = "+254712345678",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        ApplicantId = applicant.Id,
                        PhoneNumberTypeId = landlineType.Id,
                        PhoneNumber = "0201234567",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.ApplicantPhoneNumbers.AddRange(phoneNumbers);
                context.SaveChanges();
            }

            if (!context.Applications.Any())
            {
                var applicants = context.Applicants.ToList();
                var programs = context.AssistancePrograms.ToList();
                var statuses = context.Statuses.ToList();
                var officialRecords = context.OfficialRecords.ToList();

                var faker = new Faker("en");

                var applications = new List<Application>();

                int count = 20;

                for (int i = 0; i < count; i++)
                {
                    var applicant = faker.PickRandom(applicants);
                    var program = faker.PickRandom(programs);
                    var status = faker.PickRandom(statuses);
                    var officialRecord = faker.Random.Bool(0.5f) ? faker.PickRandom(officialRecords) : null;

                    applications.Add(new Application
                    {
                        ApplicationDate = DateOnly.FromDateTime(faker.Date.Past(1)),
                        ApplicantId = applicant.Id,
                        ProgramId = program.Id,
                        StatusId = status.Id,
                        OfficialRecordId = officialRecord?.Id,
                        DeclarationDate = officialRecord != null ? DateOnly.FromDateTime(faker.Date.Recent(30)) : null,
                        Removed = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                context.Applications.AddRange(applications);
                context.SaveChanges();
            }



        }
    }
}
