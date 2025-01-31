 ფიზიკური პირების ცნობარი API

 პროექტის გაშვება

1. გადმოწერეთ პროექტი
2. შეცვალეთ მონაცემთა ბაზის კონფიგურაცია "appsettings.json" ფაილში: 
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=თქვენი_სერვერი;Database=თქვენი_ბაზა;User Id=მომხმარებელი;Password=პაროლი;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   

3. გაუშვით პროექტი Visual Studio-დან

Development მოდში გაშვებისას პროექტი ავტომატურად:
- შექმნის მონაცემთა ბაზას
- გააკეთებს მიგრაციებს
- ჩაწერს საწყის მონაცემებს (seed data)

ანუ Package Manager Console-დან "Update-Database" ბრძანების გაშვება არ არის საჭირო.

 API ენდპოინტები

 პიროვნებები (Persons)
- POST "/api/persons" - ახალი პიროვნების დამატება
- PUT "/api/persons/{id}" - პიროვნების მონაცემების განახლება
- DELETE "/api/persons/{id}" - პიროვნების წაშლა
- GET "/api/persons/{id}" - პიროვნების დეტალების ნახვა
- GET "/api/persons" - პიროვნებების სიის მიღება (ფილტრაცია და გვერდებად დაყოფა)
- POST "/api/persons/{id}/image" - პიროვნების სურათის ატვირთვა
- POST "/api/persons/{id}/related-persons" - დაკავშირებული პირის დამატება
- DELETE "/api/persons/{id}/related-persons/{relatedPersonId}" - დაკავშირებული პირის წაშლა
- GET "/api/persons/{id}/related-persons/report" - დაკავშირებული პირების ანგარიში

 ქალაქები (Cities)
- GET "/api/cities" - ყველა ქალაქის სიის მიღება

 გამოყენებული ტექნოლოგიები

- ASP.NET Core
- Entity Framework Core
- Microsoft SQL Server
- Clean Architecture
- FluentValidation

API დოკუმენტაცია ხელმისაწვდომია Swagger-ის საშუალებით "/swagger" მისამართზე.
