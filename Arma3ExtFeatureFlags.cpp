
#include <cstdint>

enum DllExtensionFeatureFlags : uint64_t
{
	RVFeature_ContextArgumentsVoidPtr = 1 << 0,
	RVFeature_ContextStackTrace = 1 << 1,
	RVFeature_ContextNoDefaultCall = 1 << 2,
	RVFeature_ArgumentNoEscapeString = 1 << 3, //Ignored in Arma 3 versions prior to 2.22
};

extern "C"
{
	// Required - can be set to 0 or some combo as below depending on needs
	__declspec(dllexport) uint64_t RVExtensionFeatureFlags = RVFeature_ArgumentNoEscapeString | RVFeature_ContextNoDefaultCall; 

	// Optional Getter / Setter that can be used by C# code - omit if you don't intend to change the value at runtime
	__declspec(dllexport) uint64_t __cdecl GetRVExtensionFeatureFlags() {
		return RVExtensionFeatureFlags;
	}
	__declspec(dllexport) void __cdecl SetRVExtensionFeatureFlags(uint64_t value) {
		RVExtensionFeatureFlags = value;
	}
}