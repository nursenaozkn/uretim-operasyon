# --- Derleme aşaması ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Önce csproj'u kopyalayıp restore et (katman önbelleği için).
COPY ["UretimOperasyon.csproj", "./"]
RUN dotnet restore "UretimOperasyon.csproj"

# Geri kalan kaynakları kopyala ve yayınla.
COPY . .
RUN dotnet publish "UretimOperasyon.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Çalışma aşaması ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
# Render dinlenecek portu PORT ortam değişkeniyle verir; Program.cs bunu okur.
EXPOSE 10000
ENTRYPOINT ["dotnet", "UretimOperasyon.dll"]
