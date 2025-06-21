# 🌱 Kern

> ⚠️ **Warning: This library is under active development.**
>
> APIs, structures, and behaviors may change without notice. Not recommended for production use until a stable version is released.

**Kern** is a modular utility library for .NET that simplifies core backend patterns like **asynchronous pipelines**, **concurrent-safe queues**, **AWS S3 access**, and **error modeling**. It also offers clean ASP.NET Core integration for **standardized responses** and **FluentValidation filtering**.

## ✨ Features

### 🧠 Pipeline

A simple, composable pipeline runner for chaining asynchronous processing steps with support for early exits via `PipelineResult`.

```csharp
var pipeline = Pipeline
    .Create<string>()
    .AddStep(async input =>
    {
        if (string.IsNullOrWhiteSpace(input))
            return PipelineResult<string>.Fail(new InvalidInputError("Empty input"));

        return PipelineResult<string>.Success(input.ToUpper());
    });

var result = await pipeline.Run("hello");
```

### 📥 Queue

Thread-safe task queue manager that allows **parallel processing by group** but ensures **serial execution within the same group**, ideal for background job orchestration.

```csharp
await queue.QueueTaskAsync<SomeTask>();
```

### ☁️ S3 Helper

Abstraction over AWS S3 for uploading, downloading, and deleting objects — supports both streams and file paths, and can generate presigned URLs.

```csharp
await s3.GetObjectAsync(objectName);
```

## 📄 License

Licensed under the **GNU General Public License v3.0**. See [LICENSE](LICENSE) for full terms.
