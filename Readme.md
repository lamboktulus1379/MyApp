### MyApp

There are four services in this App.

1. MyAppApigateway.API acts as api gateway. Api gateway uses Jwt authentication for every request to user-service, transaction-service, wallet-service.
2. Auth Service for register and login user.
3. User Service for getting users and validate for specific id.
4. Transaction service for doing some transactions like payment. When user doing transaction, first its checks user's balance by hitting user service with Rest API. If it is enough, then transaction service then push transaction data into wallet service using Kafka.
5. Wallet service for handling user balance and also stores user balance log. Wallet service store user's mutations logs, which consume from transactions service.


## Detail of API
1. Register User
```
curl --location 'https://localhost:7133/api/auth/register-identity' \
--header 'Content-Type: application/json' \
--data-raw '{
    "firstName": "Test",
    "lastName": "Dua",
    "password": "MyPassword_123",
    "userName": "test22@gmail.com",
    "email": "test22@gmail.com",
    "gender": "M",
    "age": 24,
    "dateOfBirth": "1995-11-20"
}'
```

2. Login User
- Request
```
curl --location 'https://localhost:7133/api/auth/login-identity' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "test22@gmail.com",
    "password": "MyPassword_123"
}'
```
- Response:
```
{
    "IsAuthSuccessful": true,
    "ErrorMessage": null,
    "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4",
    "Is2StepVerificationRequired": false,
    "Provider": null,
    "RefreshToken": null
}
```

3. Create Transactions
- Request Success
```
curl --location 'https://localhost:7133/api/transactions' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJsYW1ib2t0dWx1czIyQGdtYWlsLmNvbSIsImV4cCI6MTY5NDQyMzg3NSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAxLyxodHRwOi8vbG9jYWxob3N0OjUwMDMvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwNS8iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDEvIn0.MW4qKZltVF8iDvUcrCa787peCCe7kF80SSSxOjfM6JA' \
--data '{
    "userId": "06884733-ecbb-4641-befd-30e26e9e9b01",
    "supplierId": "string",
    "paymentMethod": "1",
    "amount": 1000,
    "currency": "IDR"
}'
```
- Response Success
```
{
    "ResponseMessage": "OK",
    "ResponseCode": "200",
    "Data": {
        "UserId": "06884733-ecbb-4641-befd-30e26e9e9b01",
        "SupplierId": "956d8711-bea5-4470-8a75-16913647598c",
        "PaymentMethod": "1",
        "Amount": 1000,
        "Currency": "IDR"
    }
}
```

- Request Failed
```
curl --location 'https://localhost:7133/api/transactions' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4' \
--data '{
    "userId": "06884733-ecbb-4641-befd-30e26e9e9b01",
    "supplierId": "string",
    "paymentMethod": "1",
    "amount": 100000,
    "currency": "IDR"
}'
```
- Response Failed
```
{
    "ResponseMessage": "Unsufficient balance",
    "ResponseCode": "400",
    "Data": {
        "UserId": "06884733-ecbb-4641-befd-30e26e9e9b01",
        "SupplierId": "12ad475a-2785-427d-9ec8-a9e3ac8fb8a4",
        "PaymentMethod": "1",
        "Amount": 100000,
        "Currency": "IDR"
    }
}
```

4. Get Users
- Request
```
curl --location 'https://localhost:7133/api/users' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4'
```
- Response
```
{
    "ResponseMessage": "OK",
    "ResponseCode": "200",
    "Data": [
        {
            "Id": "d467706d-929a-49d5-9bbf-8dfee1411131",
            "FirstName": "Test",
            "LastName": "Dua",
            "Email": null,
            "UserName": "test22@gmail.com",
            "Gender": "M",
            "NormalizedEmail": "TEST22@GMAIL.COM",
            "NormalizedUserName": "TEST22@GMAIL.COM",
            "CreatedAt": "9/11/2023 11:51:24 AM",
            "Balance": 0,
            "Roles": [
                "Viewer"
            ]
        }
    ]
}
```

5. Get User By Id
- Request 
```
curl --location 'https://localhost:7133/api/users/d467706d-929a-49d5-9bbf-8dfee1411131' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4'
```
- Resposne
```
{
    "ResponseMessage": "OK",
    "ResponseCode": "200",
    "Data": {
        "Id": "d467706d-929a-49d5-9bbf-8dfee1411131",
        "FirstName": "Test",
        "LastName": "Dua",
        "Email": "test22@gmail.com",
        "UserName": "test22@gmail.com",
        "Gender": "M",
        "NormalizedEmail": "TEST22@GMAIL.COM",
        "NormalizedUserName": "TEST22@GMAIL.COM",
        "CreatedAt": "9/11/2023 11:51:24 AM",
        "Balance": 0,
        "Roles": ["Viewer"]
    }
}
```

6. Update Balance
- Request
```
curl --location --request PUT 'https://localhost:7133/api/users/balance' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4' \
--data '{
    "userId": "d467706d-929a-49d5-9bbf-8dfee1411131",
    "balance": 100000
}'
```
- Resposne
```
204 No Content
```

7. Get Transactions
- Request
```
curl --location 'https://localhost:7133/api/transactions' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MjJAZ21haWwuY29tIiwiZXhwIjoxNjk0NDI2NDk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDEvLGh0dHA6Ly9sb2NhbGhvc3Q6NTAwMy8saHR0cDovL2xvY2FsaG9zdDo1MDA1LyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMS8ifQ.hUW2MDQV73eYcBw4Ato5VmrWEnhNHbRvkLvYGG0WHf4'
```
- Response
```
{
    "ResponseMessage": "Success",
    "ResponseCode": "200",
    "Data": [
        {
            "UserId": "bca990e1-0916-410b-9b15-d59d39f2890f",
            "SupplierId": "d3483005-02db-401b-afd8-ef997a02abaa",
            "PaymentMethod": "1",
            "Amount": 1000,
            "Currency": "IDR"
        }
    ]
}
```

Note:
Need to update environment variable when running service:
```
export KAFKA_BROKER=pkc-4j8dq.southeastasia.azure.confluent.cloud:9092
export KAFKA_USERNAME=KLOVXE4G4V5PTAHF
export KAFKA_PASSWORD=UOvNpd4S502mvTb5C2qkEd87WvqDzzOP0lApZGLdbGH7FjgXD4a3eXDXRKOql+fc
export KAFKA_TOPIC=topic_0
export KAFKA_CONSUMER_GROUP=gra
```