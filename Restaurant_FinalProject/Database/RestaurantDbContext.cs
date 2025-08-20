using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurant_FinalProject.Data;
using System.Diagnostics;

namespace Restaurant_FinalProject.Database
{
    
        public class RestaurantDbContext : DbContext
        {
            public DbSet<Person> Persons { get; set; }
            public DbSet<Employee> Employees { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Supplier> Suppliers { get; set; }
            public DbSet<InventoryItem> InventoryItems { get; set; }
            public DbSet<Data.MenuItem> MenuItems { get; set; }
            public DbSet<Table> Tables { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }
            public DbSet<Timesheet> Timesheets { get; set; }
            public DbSet<Report> Reports { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                try
                {
                    string databasePath = Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");
                    optionsBuilder.UseSqlite($"Filename={databasePath}");

                    Debug.WriteLine($"Database path: {databasePath}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error configuring database: {ex.Message}");
                    throw;
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                try
                {
                    // Configure Person entity
                    modelBuilder.Entity<Person>(entity =>
                    {
                        entity.HasKey(p => p.PersonID);
                        entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                        entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                        entity.Property(p => p.PhoneNumber).HasMaxLength(15);
                    });

                    // Configure Employee entity
                    modelBuilder.Entity<Employee>(entity =>
                    {
                        entity.HasKey(e => e.EmployeeID);
                        entity.Property(e => e.Position).IsRequired().HasMaxLength(50);
                        entity.Property(e => e.HourlyRate).IsRequired();
                        entity.Property(e => e.Email).HasMaxLength(100);

                        // Relationship: Employee has many Timesheets
                        entity.HasMany(e => e.Timesheets)
                              .WithOne(t => t.Employee)
                              .HasForeignKey(t => t.EmployeeID)
                              .OnDelete(DeleteBehavior.Cascade);
                    });

                    // Configure Customer entity
                    modelBuilder.Entity<Customer>(entity =>
                    {
                        entity.HasKey(c => c.CustomerID);

                        // Relationship: Customer has many Reservations
                        entity.HasMany(c => c.Reservations)
                              .WithOne(r => r.Customer)
                              .HasForeignKey(r => r.CustomerID)
                              .OnDelete(DeleteBehavior.Cascade);

                        // Relationship: Customer has many Orders
                        entity.HasMany(c => c.Orders)
                              .WithOne(o => o.Customer)
                              .HasForeignKey(o => o.CustomerID)
                              .OnDelete(DeleteBehavior.Cascade);
                    });

                    // Configure Supplier entity
                    modelBuilder.Entity<Supplier>(entity =>
                    {
                        entity.HasKey(s => s.SupplierID);
                        entity.Property(s => s.CompanyName).IsRequired().HasMaxLength(100);
                        entity.Property(s => s.ContactInfo).IsRequired().HasMaxLength(200);

                        // Relationship: Supplier has many InventoryItems
                        entity.HasMany(s => s.SuppliedItems)
                              .WithOne(i => i.Supplier)
                              .HasForeignKey(i => i.SupplierID)
                              .OnDelete(DeleteBehavior.SetNull);
                    });

                    // Configure InventoryItem entity
                    modelBuilder.Entity<InventoryItem>(entity =>
                    {
                        entity.HasKey(i => i.ItemID);
                        entity.Property(i => i.ItemName).IsRequired().HasMaxLength(100);
                        entity.Property(i => i.Quantity).IsRequired();
                        entity.Property(i => i.Unit).IsRequired().HasMaxLength(20);
                        entity.Property(i => i.Threshold).IsRequired();
                        entity.Property(i => i.CostPerUnit).IsRequired();

                        // Relationship: InventoryItem has many MenuItems
                        entity.HasMany(i => i.MenuItems)
                              .WithOne(m => m.InventoryItem)
                              .HasForeignKey(m => m.ItemID)
                              .OnDelete(DeleteBehavior.SetNull);
                    });

                    // Configure MenuItem entity
                    modelBuilder.Entity<Data.MenuItem>(entity =>
                    {
                        entity.HasKey(m => m.MenuItemID);
                        entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                        entity.Property(m => m.Description).HasMaxLength(500);
                        entity.Property(m => m.Price).IsRequired();
                        entity.Property(m => m.Category).IsRequired().HasMaxLength(50);
                        entity.Property(m => m.PrepTime).HasMaxLength(20);
                        entity.Property(m => m.ImageURL).HasMaxLength(200);

                        // Relationship: MenuItem has many OrderItems
                        entity.HasMany(m => m.OrderItems)
                              .WithOne(oi => oi.MenuItem)
                              .HasForeignKey(oi => oi.MenuItemID)
                              .OnDelete(DeleteBehavior.Restrict);
                    });

                    // Configure Table entity
                    modelBuilder.Entity<Table>(entity =>
                    {
                        entity.HasKey(t => t.TableID);
                        entity.Property(t => t.Capacity).IsRequired();
                        entity.Property(t => t.IsAvailable).IsRequired();

                        // Relationship: Table has many Reservations
                        entity.HasMany(t => t.Reservations)
                              .WithOne(r => r.Table)
                              .HasForeignKey(r => r.TableID)
                              .OnDelete(DeleteBehavior.Restrict);

                        // Relationship: Table has many Orders
                        entity.HasMany(t => t.Orders)
                              .WithOne(o => o.Table)
                              .HasForeignKey(o => o.TableID)
                              .OnDelete(DeleteBehavior.SetNull);
                    });

                    // Configure Reservation entity
                    modelBuilder.Entity<Reservation>(entity =>
                    {
                        entity.HasKey(r => r.ReservationID);
                        entity.Property(r => r.ReservationDate).IsRequired();
                        entity.Property(r => r.NumGuests).IsRequired();
                        entity.Property(r => r.Status).IsRequired().HasMaxLength(20);

                        entity.HasIndex(r => r.ReservationDate);
                    });

                    // Configure Order entity
                    modelBuilder.Entity<Order>(entity =>
                    {
                        entity.HasKey(o => o.OrderID);
                        entity.Property(o => o.OrderDate).IsRequired();
                        entity.Property(o => o.Status).IsRequired().HasMaxLength(20);
                        entity.Property(o => o.TotalAmount).IsRequired();

                        // Relationship: Order has many OrderItems
                        entity.HasMany(o => o.OrderItems)
                              .WithOne(oi => oi.Order)
                              .HasForeignKey(oi => oi.OrderID)
                              .OnDelete(DeleteBehavior.Cascade);

                        entity.HasIndex(o => o.OrderDate);
                    });

                    // Configure OrderItem entity
                    modelBuilder.Entity<OrderItem>(entity =>
                    {
                        entity.HasKey(oi => oi.OrderItemID);
                        entity.Property(oi => oi.Quantity).IsRequired();
                        entity.Property(oi => oi.Price).IsRequired();
                    });

                    // Configure Timesheet entity
                    modelBuilder.Entity<Timesheet>(entity =>
                    {
                        entity.HasKey(t => t.TimesheetID);
                        entity.Property(t => t.Date).IsRequired();
                        entity.Property(t => t.HoursWorked).IsRequired();
                        entity.Property(t => t.Rate).IsRequired();
                        entity.Property(t => t.DaysWorked).IsRequired();

                        entity.HasIndex(t => t.Date);
                    });

                    // Configure Report entity
                    modelBuilder.Entity<Report>(entity =>
                    {
                        entity.HasKey(r => r.ReportID);
                        entity.Property(r => r.Month).IsRequired();
                        entity.Property(r => r.MonthlySpent).IsRequired();
                        entity.Property(r => r.LabourSpending).IsRequired();
                        entity.Property(r => r.InventorySpending).IsRequired();
                        entity.Property(r => r.Rent).IsRequired();
                        entity.Property(r => r.MonthlyReceived).IsRequired();
                        entity.Property(r => r.DateGenerated).IsRequired();

                        entity.HasIndex(r => r.Month);
                    });

                    Debug.WriteLine("Database model configured successfully");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error configuring database model: {ex.Message}");
                    throw;
                }
            }

            public async Task InitializeDatabaseAsync()
            {
                try
                {
                    await Database.EnsureCreatedAsync();

                    if (!Persons.Any())
                    {
                        Debug.WriteLine("Seeding database with sample data...");
                        await SeedDataAsync();
                        Debug.WriteLine("Database seeded successfully");
                    }
                    else
                    {
                        Debug.WriteLine("Database already contains data, skipping seeding");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error initializing database: {ex.Message}");
                    throw;
                }
            }

            private async Task SeedDataAsync()
            {
                try
                {
                    // Create sample suppliers
                    var supplier1 = new Supplier
                    {
                        CompanyName = "Fresh Produce Co.",
                        ContactInfo = "contact@freshproduce.com, 555-0101"
                    };

                    var supplier2 = new Supplier
                    {
                        CompanyName = "Quality Meats Ltd.",
                        ContactInfo = "orders@qualitymeats.com, 555-0102"
                    };

                    await Suppliers.AddRangeAsync(supplier1, supplier2);
                    await SaveChangesAsync();

                    // Create sample inventory items
                    var inventoryItems = new List<InventoryItem>
                {
                    new InventoryItem
                    {
                        ItemName = "Tomatoes",
                        Quantity = 50,
                        Unit = "kg",
                        Threshold = 10,
                        ExpirationDate = DateTime.Now.AddDays(7),
                        DeliveredDate = DateTime.Now.AddDays(-1),
                        CostPerUnit = 2.50m,
                        SupplierID = supplier1.SupplierID
                    },
                    new InventoryItem
                    {
                        ItemName = "Beef",
                        Quantity = 25,
                        Unit = "kg",
                        Threshold = 5,
                        ExpirationDate = DateTime.Now.AddDays(14),
                        DeliveredDate = DateTime.Now.AddDays(-2),
                        CostPerUnit = 12.75m,
                        SupplierID = supplier2.SupplierID
                    },
                    new InventoryItem
                    {
                        ItemName = "Lettuce",
                        Quantity = 30,
                        Unit = "heads",
                        Threshold = 8,
                        ExpirationDate = DateTime.Now.AddDays(5),
                        DeliveredDate = DateTime.Now.AddDays(-1),
                        CostPerUnit = 1.20m,
                        SupplierID = supplier1.SupplierID
                    }
                };

                    await InventoryItems.AddRangeAsync(inventoryItems);
                    await SaveChangesAsync();

                    // Create sample menu items
                    var menuItems = new List<Data.MenuItem>
                {
                    new Data.MenuItem
                    {
                        Name = "Classic Burger",
                        Description = "Juicy beef patty with fresh vegetables",
                        Price = 12.99m,
                        Category = "Main Course",
                        Availability = true,
                        PrepTime = "15 min",
                        ItemID = inventoryItems[1].ItemID
                    },
                    new Data.MenuItem
                    {
                        Name = "Fresh Salad",
                        Description = "Mixed greens with tomatoes and dressing",
                        Price = 8.50m,
                        Category = "Appetizer",
                        Availability = true,
                        PrepTime = "10 min",
                        ItemID = inventoryItems[2].ItemID
                    },
                    new Data.MenuItem
                    {
                        Name = "Tomato Soup",
                        Description = "Creamy tomato soup with herbs",
                        Price = 6.75m,
                        Category = "Soup",
                        Availability = true,
                        PrepTime = "20 min",
                        ItemID = inventoryItems[0].ItemID
                    }
                };

                    await MenuItems.AddRangeAsync(menuItems);
                    await SaveChangesAsync();

                    // Create sample employees
                    var employees = new List<Employee>
                {
                    new Employee
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        PhoneNumber = "555-0201",
                        DateHired = DateTime.Now.AddMonths(-6),
                        Position = "Manager",
                        HourlyRate = 25.50m,
                        Email = "john.smith@restaurant.com"
                    },
                    new Employee
                    {
                        FirstName = "Sarah",
                        LastName = "Johnson",
                        PhoneNumber = "555-0202",
                        DateHired = DateTime.Now.AddMonths(-3),
                        Position = "Wait Staff",
                        HourlyRate = 15.75m,
                        Email = "sarah.johnson@restaurant.com"
                    }
                };

                    await Employees.AddRangeAsync(employees);
                    await SaveChangesAsync();

                    // Create sample customers
                    var customers = new List<Customer>
                {
                    new Customer
                    {
                        FirstName = "Michael",
                        LastName = "Brown",
                        PhoneNumber = "555-0301"
                    },
                    new Customer
                    {
                        FirstName = "Emily",
                        LastName = "Davis",
                        PhoneNumber = "555-0302"
                    }
                };

                    await Customers.AddRangeAsync(customers);
                    await SaveChangesAsync();

                    // Create sample tables
                    var tables = new List<Table>
                {
                    new Table { Capacity = 4, IsAvailable = true },
                    new Table { Capacity = 2, IsAvailable = true },
                    new Table { Capacity = 6, IsAvailable = false },
                    new Table { Capacity = 4, IsAvailable = true }
                };

                    await Tables.AddRangeAsync(tables);
                    await SaveChangesAsync();

                    // Create sample reservations
                    var reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        ReservationDate = DateTime.Now.AddHours(2),
                        NumGuests = 4,
                        Status = "Confirmed",
                        CustomerID = customers[0].CustomerID,
                        TableID = tables[0].TableID
                    },
                    new Reservation
                    {
                        ReservationDate = DateTime.Now.AddDays(1).AddHours(3),
                        NumGuests = 2,
                        Status = "Pending",
                        CustomerID = customers[1].CustomerID,
                        TableID = tables[1].TableID
                    }
                };

                    await Reservations.AddRangeAsync(reservations);
                    await SaveChangesAsync();

                    // Create sample orders
                    var orders = new List<Order>
                {
                    new Order
                    {
                        OrderDate = DateTime.Now.AddHours(-1),
                        Status = "Completed",
                        TotalAmount = 25.48m,
                        CustomerID = customers[0].CustomerID,
                        TableID = tables[0].TableID
                    },
                    new Order
                    {
                        OrderDate = DateTime.Now,
                        Status = "In Progress",
                        TotalAmount = 19.25m,
                        CustomerID = customers[1].CustomerID,
                        TableID = tables[1].TableID
                    }
                };

                    await Orders.AddRangeAsync(orders);
                    await SaveChangesAsync();

                    // Create sample order items
                    var orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Quantity = 2,
                        Price = 12.99m,
                        OrderID = orders[0].OrderID,
                        MenuItemID = menuItems[0].MenuItemID
                    },
                    new OrderItem
                    {
                        Quantity = 1,
                        Price = 8.50m,
                        OrderID = orders[1].OrderID,
                        MenuItemID = menuItems[1].MenuItemID
                    }
                };

                    await OrderItems.AddRangeAsync(orderItems);
                    await SaveChangesAsync();

                    // Create sample timesheets
                    var timesheets = new List<Timesheet>
                {
                    new Timesheet
                    {
                        Date = DateTime.Now.Date,
                        HoursWorked = 8.0m,
                        Rate = employees[0].HourlyRate,
                        DaysWorked = 1,
                        ShiftStart = new TimeSpan(9, 0, 0),
                        ShiftEnd = new TimeSpan(17, 0, 0),
                        EmployeeID = employees[0].EmployeeID
                    },
                    new Timesheet
                    {
                        Date = DateTime.Now.Date,
                        HoursWorked = 6.5m,
                        Rate = employees[1].HourlyRate,
                        DaysWorked = 1,
                        ShiftStart = new TimeSpan(10, 0, 0),
                        ShiftEnd = new TimeSpan(16, 30, 0),
                        EmployeeID = employees[1].EmployeeID
                    }
                };

                    await Timesheets.AddRangeAsync(timesheets);
                    await SaveChangesAsync();

                    // Create sample report
                    var report = new Report
                    {
                        Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                        MonthlySpent = 2500.00m,
                        LabourSpending = 3200.00m,
                        InventorySpending = 1800.00m,
                        Rent = 2000.00m,
                        MonthlyReceived = 12500.00m,
                        DateGenerated = DateTime.Now
                    };

                    await Reports.AddAsync(report);
                    await SaveChangesAsync();

                    Debug.WriteLine("Sample data seeded successfully");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error seeding data: {ex.Message}");
                    throw;
                }
            }
        }
    }

        
    