FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install NativeAOT build prerequisites
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       clang zlib1g-dev

WORKDIR /source

ADD CloudflareDns .

RUN dotnet publish -o /app -r linux-x64 -c Release


FROM cgr.dev/chainguard/wolfi-base

RUN apk add --no-cache \
    dotnet-8-runtime

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["/app/CloudflareDns"]
