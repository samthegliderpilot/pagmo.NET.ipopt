# Changelog

## v1.0.0-beta.6

### Highlights

- Initial release as a standalone repository, split from `pagmoNet.ipopt`.
- Windows x64, Linux x64, and macOS (arm64 + x86_64 universal binary) are all supported via CI.
- NUnit test suite covering availability, instantiation, option setting, evolve improvement, and log extraction.
- `InternalsVisibleTo` grant in `Pagmo.NET` enables the add-on to access SWIG-internal plumbing from a separate assembly.

### Known limitations

- `GetLastOptimizationResultCode()` may return `-12` (Invalid_Option / HSL loader artefact) even when the solver successfully improves the objective. This is not a functional regression.
- MUMPS and SPRAL linear solvers are not included in this build; use `ma27`, `ma57`, `ma86`, `ma97`, or `pardiso`.
