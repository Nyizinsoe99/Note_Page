# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# project file ကို copy ကူးပြီး restore လုပ်မယ်
COPY ["Node_Page.csproj", "./"]
RUN dotnet restore "./Node_Page.csproj"

# ဖိုင်တွေအကုန်လုံးကို copy ကူးမယ်
COPY . .

# ⚠️ အရေးကြီးဆုံးအဆင့်: node folder ထဲက ကုဒ်တွေကို root ထဲ ရောက်အောင် ထုတ်ပေးတာ
RUN cp -r node/* . || true

# ပြန်ပြီး Build မယ်
RUN dotnet build "Node_Page.csproj" -c Release -o /app/build

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "Node_Page.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Node_Page.dll"]