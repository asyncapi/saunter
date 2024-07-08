# Contributing

Feel free to get involved in the project by opening issues, or submitting pull requests.


## Build & Release Process

Builds and releases managed with github actions.

### Build

* Local builds are as simple as `dotnet build && dotnet test`
* CI builds,fmt,unit on every push
* [.github/workflows/ci.yaml](./.github/workflows/ci.yaml)
* Build and tests MUST pass before merging to master

### Release

* Locally, packages can be created using `dotnet pack ./src/Saunter/Saunter.csproj`
* Release packages are built from github actions
* [.github/workflows/release.yaml](./.github/workflows/release.yaml)
* Pushing a tag formatted as `v*.*.*` will trigger the release workflow
* Releases MUST use semantic versioning
* Tags for release (non-preproduction) should relate to commits on `master`
* The repository owner is responsible for tagging and releasing
* Packages are pushed to [nuget.org/packages/saunter](https://www.nuget.org/packages/saunter)

