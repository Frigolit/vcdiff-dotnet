// UnmanagedClass.h

#pragma once

#include <memory>
#include <vector>

#include "google/vcdecoder.h"
#include "google/vcencoder.h"

namespace VCDiffDotNet {
		
	class VCDiffWrapper
	{
	private:
		open_vcdiff::VCDiffStreamingEncoder *encoder;
		open_vcdiff::VCDiffStreamingDecoder *decoder;
		std::auto_ptr<open_vcdiff::HashedDictionary> hashed_dictionary_;

		open_vcdiff::VCDiffFormatExtensionFlags format_flags;

		std::vector<char> sourceDataBuffer;

		bool allow_vcd_target;
		bool target_matches;
		size_t max_target_file_size;
		size_t max_target_window_size;

	public:
		VCDiffWrapper();

		void Settings(
					bool allow_vcd_target,
					bool checksum,
					bool interleaved,
					bool json,
					bool target_matches,
					size_t max_target_file_size,
					size_t max_target_window_size);


		void SetEncodingFormatFlags(bool checksum, bool interleaved, bool json, bool target_matches);
		void SetAllowVcdTargetFlag(bool value);
		void SetTargetMatches(bool value);
		void SetMaxTargetFileSize(size_t value);
		void SetMaxTargetWindowSize(size_t value);

		void SetSource(const char* sourceBuff, size_t sourceBuffSize);

		bool InitEncoder();
		bool StartEncoding(const char **buff, size_t *size);
		bool EncodeChunk(const char* data, size_t len, const char **buff, size_t *size);
		bool FinishEncoding(const char **buff, size_t *size);

		void InitDecoder();
		void StartDecoding();
		bool DecodeChunk(const char* data, size_t len, const char **buff, size_t *size);
		bool FinishDecoding();

	}; // class VCDiffWrapper

} // namespace vcdiffdotnet