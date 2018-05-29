FROM gcr.io/google-appengine/aspnetcore:2.0.7
#FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY . /app
ENV ASPNETCORE_URLS http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "work.dll"]