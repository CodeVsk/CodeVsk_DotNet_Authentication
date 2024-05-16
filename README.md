# DotNet Entity Authentication

Authentication service developed in ASP.NET Core 8 with Domain Driven Design principles.

## Authors

- [@CodeVsk](https://www.github.com/codevsk)

## Stacks

**Back-end:** ASP.NET Core 8

**Cache:** Redis

**Database:** Microsoft SQL Server

**Infra:**: Docker

## Redis Setup

Install redis image

```bash
  docker pull redis
```

Run redis container

```bash
  docker run --name redis-authentication -d -p 6379:6379 redis redis-server --requirepass dev
```
