import binascii
import hmac
import struct
import argparse
import zlib

parser = argparse.ArgumentParser(description='Process A FCKING GRP PACKET FCKING HATE THIS BULLSHIT.')
parser.add_argument('hex', metavar='N', type=str, nargs='+', help='The hex')
args = parser.parse_args()
hexval = args.hex[0]
data = bytearray.fromhex(hexval)
data = zlib.decompress(data)
final = ''.join('{:02x}'.format(x) for x in data)
print(final)